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
			
			BoxCollider2D collisionsCollider = GetComponents<BoxCollider2D>()[0];
			BoxCollider2D triggerCollider = GetComponents<BoxCollider2D>()[1];
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
			
			BoxCollider2D collisionsCollider = GetComponents<BoxCollider2D>()[0];
			BoxCollider2D triggerCollider = GetComponents<BoxCollider2D>()[1];
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
		BoxCollider2D collider = GetComponents<BoxCollider2D>()[0];

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

				open(true);

				//disable trigger collider
				BoxCollider2D collider = GetComponents<BoxCollider2D>()[1];
				collider.enabled = false;
			}
		}
		
	} 
}
