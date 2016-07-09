using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Node;

public class NoteBehavior : BaseEntityBehavior {

	public Note note {
		get {
			return (Note) entity;
		}
	}

	public void init(Note note) {

		base.init(note);

	}


	protected override void updateBehavior() {

		getTriggerActionInCollider().enabled = note.hasTriggerActionCollider;
		getTriggerActionOutCollider().enabled = note.hasTriggerActionCollider;
		getTriggerMessageOutCollider().enabled = note.hasTriggerMessageOutCollider;

	}


	private CircleCollider2D getTriggerActionInCollider() {
		return GetComponents<CircleCollider2D>()[0];
	}

	private CircleCollider2D getTriggerActionOutCollider() {
		return GetComponents<CircleCollider2D>()[1];
	}

	private CircleCollider2D getTriggerMessageOutCollider() {
		return GetComponents<CircleCollider2D>()[2];
	}

	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(getTriggerActionInCollider().IsTouching(collider)) {
			note.onEnterTriggerActionCollider();
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(!getTriggerActionOutCollider().IsTouching(collider)) {
			note.onExitTriggerActionCollider();
		}

		if(!getTriggerMessageOutCollider().IsTouching(collider)) {
			note.onExitTriggerMessageCollider();
		}
	}


}

