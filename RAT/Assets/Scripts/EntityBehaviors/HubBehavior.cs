using UnityEngine;
using Node;
using System;
using System.Collections;

public class HubBehavior : BaseEntityBehavior {

	private static Sprite spriteDeactivated;
	private static Sprite spriteActivated;


	public Hub hub {
		get {
			return (Hub) entity;
		}
	}

	private CircleCollider2D getTriggerActionInCollider() {
		return GetComponents<CircleCollider2D>()[0];
	}

	private CircleCollider2D getTriggerActionOutCollider() {
		return GetComponents<CircleCollider2D>()[1];
	}

	public void init(Hub hub) {

		//load the images
		if(spriteDeactivated == null) {
			Texture2D textureDeactivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Deactivated");
			spriteDeactivated = Sprite.Create(textureDeactivated, 
				new Rect(0, 0, textureDeactivated.width, textureDeactivated.height),
				new Vector2(0.25f, 0.75f),
				Constants.TILE_SIZE * 2);
		}

		if(spriteActivated == null) {
			Texture2D textureActivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Activated");
			spriteActivated = Sprite.Create(textureActivated, 
				new Rect(0, 0, textureActivated.width, textureActivated.height),
				new Vector2(0.25f, 0.75f),
				Constants.TILE_SIZE * 2);
		}

		base.init(hub);

	}

	protected override void updateBehavior() {

		if(hub.isActivated) {
			GetComponent<SpriteRenderer>().sprite = spriteActivated;
		} else {
			GetComponent<SpriteRenderer>().sprite = spriteDeactivated;
		}

		getTriggerActionInCollider().enabled = hub.hasTriggerActionCollider;
		getTriggerActionOutCollider().enabled = hub.hasTriggerActionCollider;
	}

	void OnTriggerStay2D(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}
			
		if(getTriggerActionInCollider().IsTouching(collider)) {
			hub.onEnterTriggerActionCollider();
		}

	}
	
	void OnTriggerExit2D(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(!getTriggerActionOutCollider().IsTouching(collider)) {
			hub.onExitTriggerActionCollider();
		}

	}


}
