using UnityEngine;
using System.Collections;
using Level;

public class Door : MonoBehaviour {
	
	private NodeElementDoor nodeElementDoor;
	
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
		int nbSprites = (int)(texture.width / (float)Constants.TILE_SIZE);
		sprites = new Sprite[nbSprites];

		for(int i=0 ; i<nbSprites ; i++) {
			sprites[i] = Sprite.Create(texture, 
			                          new Rect(i * spacing * Constants.TILE_SIZE, 0, spacing * Constants.TILE_SIZE, texture.height),
			                          new Vector2(0.5f, 0.5f)
			                          );
		}
		
		//update game object size
		if(orientation == NodeOrientation.Orientation.FACE) {
			transform.localScale = new Vector2(spacing * Constants.TILE_SIZE, 2 * Constants.TILE_SIZE);
		} else {
			transform.localScale = new Vector2(Constants.TILE_SIZE, spacing * Constants.TILE_SIZE);
		}

	}

	public void open(bool animated) {

		if(animated) {

			throw new System.NotImplementedException("TODO");
		
		} else {
			
			updateCollider(1);
			updateSprite(1);
		}
	}
	
	public void close(bool animated) {
		
		if(animated) {
			
			throw new System.NotImplementedException("TODO");
			
		} else {
			
			updateCollider(0);
			updateSprite(0);
		}
	}

	private void updateCollider(float percentage) {

		BoxCollider2D collider = GetComponent<BoxCollider2D>();

		collider.enabled = (percentage < 1);
	}

	private void updateSprite(float percentage) {

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		int nbSprites = sprites.Length;

		if(percentage < 0) {
			percentage = 0;
		} else if(percentage > 1) {
			percentage = 1;
		}

		spriteRenderer.sprite = sprites[(int)(percentage / (float)nbSprites)];

	}

}
