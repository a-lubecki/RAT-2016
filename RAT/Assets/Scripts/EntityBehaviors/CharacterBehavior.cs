using UnityEngine;
using System;
using System.Collections;

public abstract class CharacterBehavior : BaseEntityBehavior {

	public Character character {
		get {
			return (Character) entity;
		}
	}


	protected void init(Character character) {

		base.init(character);

	}

	public override void onBehaviorAttached() {

		transform.position = new Vector2(character.realPosX, character.realPosY);

	}

	protected override void updateBehavior() {

		if (character.isDead()) {

			//remove all colliders
			foreach(Collider2D collider in GetComponents<Collider2D>()) {
				collider.enabled = true;
			}
		}
	}

}

