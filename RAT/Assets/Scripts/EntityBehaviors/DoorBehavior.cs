using UnityEngine;
using System.Collections;
using Node;

public class DoorBehavior : BaseEntityBehavior, IActionnable {
	
	public Door door {
		get {
			return (Door) entity;
		}
	}


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

	public void init(Door door) {

		base.init(door);
		
		Orientation orientation = door.orientation;
		int spacing = door.spacing;
		
		//load the image
		string imageName = "Door.Laboratory." + spacing + "." + orientation.ToString();
		
		Texture2D texture = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + imageName);
		
		BoxCollider2D collisionsCollider = getCollisionsCollider();
		BoxCollider2D triggerCollider = getTriggerCollider();
		
		//load all sprites
		if(orientation == Orientation.FACE) {
			
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

		if(door.isOpened) {
			updateCollider(true);
			updateSprite(sprites.Length);
		} else {
			updateCollider(false);
			updateSprite(0);
		}
	}


	public void open(bool animated) {
		
		if(door.isOpened) {
			return;
		}
		
		if(animated) {
			
			StartCoroutine(animateDoor(true, 0.25f));
			
		} else {
			
			updateCollider(true);
			updateSprite(sprites.Length);

			PlayerActionsManager.Instance.hideAction(new ActionDoorOpen(this));
		}
	}
	
	public void close(bool animated) {
		
		if(!door.isOpened) {
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
			yield break;
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
		door.isOpened = isOpened;

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

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(door.isOpened) {
			return;
		}
	
		if(getTriggerCollider().IsTouching(collider)) {
			PlayerActionsManager.Instance.showAction(new ActionDoorOpen(this));
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}
		
		if(door.isOpened) {
			return;
		}

		if(!getTriggerCollider().IsTouching(collider)) {
			PlayerActionsManager.Instance.hideAction(new ActionDoorOpen(this));
		}

		if(!getTriggerOutCollider().IsTouching(collider)) {
			//remove messages if player is exiting the larger zone
			MessageDisplayer.Instance.removeAllMessagesFrom(this);
		}

	}

	void IActionnable.notifyActionShown(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionHidden(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		if(door.isOpened) {
			return;
		}

		StartCoroutine(delayPlayerAfterAction());

	}
	
	private IEnumerator delayPlayerAfterAction() {
		
		PlayerBehavior playerBehavior = GameHelper.Instance.findPlayerBehavior();
		
		playerBehavior.disableControls();
		getTriggerCollider().enabled = false;
		getTriggerOutCollider().enabled = false;

		
		yield return new WaitForSeconds(0.75f);

		manageDoorOpening();
		
		playerBehavior.enableControls();
		
		if(!door.isOpened) {
			getTriggerOutCollider().enabled = true;
		}

		yield return new WaitForSeconds(1f);

		//enable collider after delay to avoid displaying the action directly
		//with the message if the door is still closed  
		if(!door.isOpened) {
			getTriggerCollider().enabled = true;
		}

	}

	private void manageDoorOpening() {
		
		//check if the player has to be in the right side to open the door
		if(door.hasUnlockSide) {
			
			Direction unlockSide = door.unlockSide;
			
			if(unlockSide == Direction.NONE) {
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.Blocked")));
				return;
			}
			
			GameObject playerGameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER);
			
			float x = transform.position.x;
			float y = transform.position.y;
			float xPlayer = playerGameObject.transform.position.x;
			float yPlayer = playerGameObject.transform.position.y;
			
			if((unlockSide == Direction.UP && y > yPlayer) ||
			   (unlockSide == Direction.DOWN && y < yPlayer) ||
			   (unlockSide == Direction.LEFT && x < xPlayer) ||
			   (unlockSide == Direction.RIGHT && x > xPlayer)) {
				
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.WrongSide")));
				return;
			}
		}

		ItemPattern requiredItemPattern = door.requiredItemPattern;
		if(requiredItemPattern != null) {

			if(!GameManager.Instance.getInventory().hasItemWithPattern(requiredItemPattern)) {
				//door remains locked
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.Locked")));
				return;
			}

			//door unlocked with item
			MessageDisplayer.Instance.displayMessages(new Message(this, string.Format(Constants.tr("Message.Door.Unlock"), requiredItemPattern.getTrName())));

		}
		
		open(true);

	}
}

