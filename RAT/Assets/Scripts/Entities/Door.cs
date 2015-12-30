using UnityEngine;
using System.Collections;
using Level;

public class Door : MonoBehaviour, IActionnable {
	
	public NodeElementDoor nodeElementDoor { get; private set; }
	
	public bool isOpened { get; private set; }
	private bool isAnimatingDoor = false;
	
	private Sprite[] sprites;
	
	
	private BoxCollider2D getCollisionsCollider() {
		return GetComponents<BoxCollider2D>()[0];
	}
	
	private BoxCollider2D getTriggerCollider() {
		return GetComponents<BoxCollider2D>()[1];
	}
	
	private CircleCollider2D getTriggerOutCollider() {
		return GetComponent<CircleCollider2D>();
	}

	public void setNodeElementDoor(NodeElementDoor nodeElementDoor) {
		
		if(nodeElementDoor == null) {
			throw new System.InvalidOperationException();
		}
		
		this.nodeElementDoor = nodeElementDoor;
		
		
		NodeOrientation.Orientation orientation = nodeElementDoor.nodeOrientation.value;
		int spacing = nodeElementDoor.nodeSpacing.value;
		
		//load th eimage
		string imageName = "Door.Laboratory." + spacing + "." + orientation.ToString();
		
		Texture2D texture = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + imageName);
		
		BoxCollider2D collisionsCollider = getCollisionsCollider();
		BoxCollider2D triggerCollider = getTriggerCollider();
		
		//load all sprites
		if(orientation == NodeOrientation.Orientation.FACE) {
			
			int nbSprites = (int)(texture.width / (float)(spacing * Constants.TILE_SIZE));
			sprites = new Sprite[nbSprites];
			
			for(int i=0 ; i<nbSprites ; i++) {
				
				sprites[i] = Sprite.Create(texture, 
				                           new Rect(i * spacing * Constants.TILE_SIZE, 0, spacing * Constants.TILE_SIZE, texture.height),
				                           new Vector2(0.5f + (1 - spacing) * 0.5f / (float)spacing, 0.25f),
				                           Constants.TILE_SIZE);
			}
			
			collisionsCollider.size = triggerCollider.size = new Vector2(spacing, 1.25f);
			collisionsCollider.offset = triggerCollider.offset = new Vector2((spacing - 1) * 0.5f, 0.5f);
			
		} else {
			
			int nbSprites = (int)(texture.width / (float)Constants.TILE_SIZE);
			sprites = new Sprite[nbSprites];
			
			for(int i=0 ; i<nbSprites ; i++) {
				
				sprites[i] = Sprite.Create(texture, 
				                           new Rect(i * Constants.TILE_SIZE, 0, Constants.TILE_SIZE, texture.height),
				                           new Vector2(0.5f, 0.5f + (1 - spacing) * 0.5f / (float)spacing),
				                           Constants.TILE_SIZE
				                           );
			}
			
			collisionsCollider.size = triggerCollider.size = new Vector2(0.35f, spacing);
			collisionsCollider.offset = triggerCollider.offset = new Vector2(0, (spacing - 1) * 0.5f);
		}
		
		//set the door as closed
		updateCollider(false);
		updateSprite(0);
	}
	
	public void init(bool opened) {

		if(opened) {
			updateCollider(true);
			updateSprite(sprites.Length);
		} else {
			updateCollider(false);
			updateSprite(0);
		}
	}

	public void open(bool animated) {
		
		if(isOpened) {
			return;
		}
		
		if(animated) {
			
			StartCoroutine(animateDoor(true, 0.25f));
			
		} else {
			
			updateCollider(true);
			updateSprite(sprites.Length);

			PlayerActionsManager.Instance.hideAction(new ActionOpenDoor(this));

			MessageDisplayer.Instance.removeAllMessagesFrom(this);
		}
	}
	
	public void close(bool animated) {
		
		if(!isOpened) {
			return;
		}
		
		if(animated) {
			
			StartCoroutine(animateDoor(false, 0.25f));
			
		} else {
			
			updateCollider(false);
			updateSprite(0);
		}
	}
	
	IEnumerator animateDoor(bool actionOpen, float totalTime) {
		
		if(isAnimatingDoor) {
			yield return false;
		}
		
		isAnimatingDoor = true;

		if(!actionOpen) {
			//the player can't go through the door during the closing animation
			updateCollider(false);
		}

		int frame = 1;
		float deltaTime = totalTime / (float)sprites.Length;
		
		while(frame < sprites.Length) {
			
			int currentFrame = frame;
			if(!actionOpen) {
				currentFrame = sprites.Length - frame - 1;
			}

			updateSprite(currentFrame);
			
			frame++;
			
			yield return new WaitForSeconds(deltaTime);
		}
		
		if(actionOpen) {
			open(false);
		} else {
			close(false);
		}
		
		isAnimatingDoor = false;
		
	}
	
	private void updateCollider(bool isOpened) {
		
		//disable all collider
		this.isOpened = isOpened;

		Collider2D[] colliders = GetComponents<Collider2D>();
		foreach(Collider2D c in colliders) {
			c.enabled = !isOpened;
		}

	}
	
	private void updateSprite(int frame) {
		
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		
		if(frame < 0) {
			frame = 0;
		} else if(frame >= sprites.Length) {
			frame = sprites.Length - 1;
		}
		
		spriteRenderer.sprite = sprites[frame];
	}
	

	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(collider.name)) {
			return;
		}

		if(isOpened) {
			return;
		}
	
		if(getTriggerCollider().IsTouching(collider)) {
			PlayerActionsManager.Instance.showAction(new ActionOpenDoor(this));
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(collider.name)) {
			return;
		}
		
		if(isOpened) {
			return;
		}

		if(!getTriggerCollider().IsTouching(collider)) {
			PlayerActionsManager.Instance.hideAction(new ActionOpenDoor(this));
		}

		if(!getTriggerOutCollider().IsTouching(collider)) {
			//remove messages if player is exiting the larger zone
			MessageDisplayer.Instance.removeAllMessagesFrom(this);
		}

	}


	void IActionnable.notifyAction(BaseAction action) {

		if(isOpened) {
			return;
		}

		StartCoroutine(delayPlayerAfterAction());

	}
	
	private IEnumerator delayPlayerAfterAction() {
		
		PlayerCollider playerControls = GameHelper.Instance.getPlayerControls();
		
		playerControls.disableControls();
		getTriggerCollider().enabled = false;
		getTriggerOutCollider().enabled = false;

		
		yield return new WaitForSeconds(0.75f);

		manageDoorOpening();
		
		playerControls.enableControls();
		
		if(!isOpened) {
			getTriggerOutCollider().enabled = true;
		}

		yield return new WaitForSeconds(1f);

		//enable collider after delay to avoid displaying the action directly
		//with the message if the door is still closed  
		if(!isOpened) {
			getTriggerCollider().enabled = true;
		}

	}

	private void manageDoorOpening() {
		
		//check if the player has to be in the right side to open the door
		if(nodeElementDoor.nodeUnlockSide != null) {
			
			NodeDirection.Direction unlockSide = nodeElementDoor.nodeUnlockSide.value;
			
			if(unlockSide == NodeDirection.Direction.NONE) {
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.Blocked")));
				return;
			}
			
			GameObject playerGameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER);
			
			float x = transform.position.x;
			float y = transform.position.y;
			float xPlayer = playerGameObject.transform.position.x;
			float yPlayer = playerGameObject.transform.position.y;
			
			if((unlockSide == NodeDirection.Direction.UP && y > yPlayer) ||
			   (unlockSide == NodeDirection.Direction.DOWN && y < yPlayer) ||
			   (unlockSide == NodeDirection.Direction.LEFT && x < xPlayer) ||
			   (unlockSide == NodeDirection.Direction.RIGHT && x > xPlayer)) {
				
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.WrongSide")));
				return;
			}
		}
		
		if(nodeElementDoor.nodeRequire != null) {
			//TODO item
			MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.Locked")));
			return;
		}
		
		open(true);

	}
}

