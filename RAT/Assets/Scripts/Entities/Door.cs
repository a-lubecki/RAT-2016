using UnityEngine;
using System.Collections;
using Level;

public class Door : MonoBehaviour {
	
	private NodeElementDoor nodeElementDoor;
	
	private bool isOpened = false;
	private bool isAnimatingDoor = false;
	
	private Sprite[] sprites;
	
	
	public void setNodeElementDoor(NodeElementDoor nodeElementDoor) {
		
		if(nodeElementDoor == null) {
			throw new System.InvalidOperationException();
		}
		
		this.nodeElementDoor = nodeElementDoor;
		
		
		NodeOrientation.Orientation orientation = nodeElementDoor.nodeOrientation.value;
		int spacing = nodeElementDoor.nodeSpacing.value;
		
		//load th eimage
		string imageName = "Door.Laboratory." + spacing + "." + orientation.ToString() + ".png";
		
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
		updateCollider(0);
		updateSprite(0);
	}
	
	public void open(bool animated) {
		
		if(isOpened) {
			return;
		}
		
		if(animated) {
			
			StartCoroutine(animateDoor(true, 0.25f));
			
		} else {
			
			updateCollider(sprites.Length);
			updateSprite(sprites.Length);
		}
	}
	
	public void close(bool animated) {
		
		if(!isOpened) {
			return;
		}
		
		if(animated) {
			
			StartCoroutine(animateDoor(false, 0.25f));
			
		} else {
			
			updateCollider(0);
			updateSprite(0);
		}
	}
	
	IEnumerator animateDoor(bool actionOpen, float totalTime) {
		
		if(isAnimatingDoor) {
			return false;
		}
		
		isAnimatingDoor = true;
		
		int frame = 1;
		float deltaTime = totalTime / (float)sprites.Length;
		
		while(frame < sprites.Length) {
			
			int currentFrame = frame;
			if(!actionOpen) {
				currentFrame = sprites.Length - frame - 1;
			}
			
			updateCollider(currentFrame);
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
	
	private void updateCollider(int frame) {
		
		//disable collisions collider
		BoxCollider2D collider = getCollisionsCollider();
		
		isOpened = (frame >= sprites.Length - 1);
		collider.enabled = !isOpened;
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
	
	void OnTriggerEnter2D(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_CONTROLS.Equals(other.name)) {
			
			if(!isOpened) {
				
				//check if the player has to be in the right side to open the door
				if(nodeElementDoor.nodeUnlockSide != null) {
					
					NodeDirection.Direction unlockSide = nodeElementDoor.nodeUnlockSide.value;
					
					if(unlockSide == NodeDirection.Direction.NONE) {
						MessageDisplayer.Instance.displayMessage("La porte ne s'ouvre pas.");
						return;
					}
					
					float x = transform.position.x;
					float y = transform.position.y;
					float xOther = other.transform.position.x;
					float yOther = other.transform.position.y;
					
					if((unlockSide == NodeDirection.Direction.UP && y > yOther) ||
					   (unlockSide == NodeDirection.Direction.DOWN && y < yOther) ||
					   (unlockSide == NodeDirection.Direction.LEFT && x < xOther) ||
					   (unlockSide == NodeDirection.Direction.RIGHT && x > xOther)) {
						
						MessageDisplayer.Instance.displayMessage("La porte ne peut pas être ouverte de ce coté.");
						return;
					}
				}
				
				if(nodeElementDoor.nodeRequire != null) {
					//TODO item
					MessageDisplayer.Instance.displayMessage("La porte est verouillée.");
					return;
				}
				
				open(true);
				
				//disable trigger collider
				getTriggerCollider().enabled = false;
				
			}
		}
		
	} 
	
	private BoxCollider2D getCollisionsCollider() {
		return GetComponents<BoxCollider2D>()[0];
	}
	
	private BoxCollider2D getTriggerCollider() {
		return GetComponents<BoxCollider2D>()[1];
	}
}

