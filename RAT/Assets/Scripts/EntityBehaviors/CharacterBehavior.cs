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

	protected override void updateBehavior() {

		transform.position = new Vector2(character.realPosX, character.realPosY);

		if (character.isDead()) {

			//remove all colliders
			foreach(Collider2D collider in GetComponents<Collider2D>()) {
				collider.enabled = true;
			}
		}
	}


	protected override void FixedUpdate() {

		base.FixedUpdate();

		if(!isActiveAndEnabled) {
			return;
		}

		if(character == null) {
			//not prepared
			return;
		}

		if(InputsManager.Instance.isPaused) {
			return;
		}
		
		Vector2 newVector = character.getNewMoveVector();

		//update infos
		float dx = newVector.x;
		float dy = newVector.y;
		
		bool wasMoving = character.isMoving;
		
		character.isMoving = (dx != 0 || dy != 0);
		
		if(character.isMoving) {
			
			Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
			
			//update transform with int vector to move with the grid
			rigidBody.MovePosition(
				new Vector2(
				rigidBody.position.x + dx * Time.deltaTime, 
				rigidBody.position.y + dy * Time.deltaTime
				)
			);

			character.updateRealPositionAngle(wasMoving, rigidBody.position, Constants.vectorToAngle(newVector.x, newVector.y));

		}
		
	}

}

