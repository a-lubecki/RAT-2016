using UnityEngine;
using System.Collections;
using Node;

public class DoorBehavior : BaseEntityBehavior {
	
	public Door door {
		get {
			return (Door) entity;
		}
	}


	private Sprite[] sprites;


	public void init(Door door) {

		Orientation orientation = door.orientation;
		int spacing = door.spacing;
		
		//load the image
		string imageName = "Door.Laboratory." + spacing + "." + orientation.ToString();
		
		Texture2D texture = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + imageName);
		
		BoxCollider2D collisionsCollider = getCollisionsCollider();
		BoxCollider2D triggerCollider = getTriggerActionCollider();
		
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

		base.init(door);

	}

	protected override void updateBehavior() {

		//update colliders
		Collider2D[] colliders = GetComponents<Collider2D>();
		foreach(Collider2D c in colliders) {
			c.enabled = !door.isOpened;
		}

		//update frame
		int frame = (int) (door.openingPercentage * sprites.Length);

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		if(frame < 0) {
			frame = 0;
		} else if(frame >= sprites.Length) {
			frame = sprites.Length - 1;
		}

		spriteRenderer.sprite = sprites[frame];


		getTriggerActionCollider().enabled = door.hasTriggerActionCollider;
		getTriggerMessageOutCollider().enabled = door.hasTriggerMessageOutCollider;

	}


	private BoxCollider2D getCollisionsCollider() {
		return GetComponents<BoxCollider2D>()[0];
	}

	private BoxCollider2D getTriggerActionCollider() {
		return GetComponents<BoxCollider2D>()[1];
	}

	private CircleCollider2D getTriggerMessageOutCollider() {
		return GetComponent<CircleCollider2D>();
	}

	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(getTriggerActionCollider().IsTouching(collider)) {
			door.onEnterTriggerActionCollider();
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return; 
		}

		if(!getTriggerActionCollider().IsTouching(collider)) {
			door.onExitTriggerActionCollider();
		}

		if(!getTriggerMessageOutCollider().IsTouching(collider)) {
			door.onExitTriggerMessageOutCollider();
		}

	}

}

