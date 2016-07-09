using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node;

public class LootBehavior : BaseEntityBehavior {

	public Loot loot {
		get {
			return (Loot) entity;
		}
	}


	public void init(Loot loot) {

		base.init(loot);

	}

	public override void onBehaviorAttached() {

		if (!loot.isCollected) {
			GetComponent<Gif>().startAnimation();
		}
	}

	public override void onBehaviorDetached() {

		GetComponent<Gif>().stopAnimation();

	}

	protected override void updateBehavior() {

		getTriggerActionInCollider().enabled = loot.hasTriggerActionCollider;
		getTriggerActionOutCollider().enabled = loot.hasTriggerActionCollider;

		if (loot.isCollected) {

			getTriggerActionInCollider().enabled = false;

			GetComponent<Gif>().stopAnimation();

			//hide the image, can't disable and destroy the object because it won't be saved with the collected items
			GetComponent<SpriteRenderer>().sprite = null;
		}
	}


	private CircleCollider2D getTriggerActionInCollider() {
		return GetComponents<CircleCollider2D>()[0];
	}

	private CircleCollider2D getTriggerActionOutCollider() {
		return GetComponents<CircleCollider2D>()[1];
	}

	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(getTriggerActionInCollider().IsTouching(collider)) {
			loot.onEnterTriggerActionCollider();
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(!getTriggerActionOutCollider().IsTouching(collider)) {
			loot.onExitTriggerActionCollider();
		}

	}


}

