using UnityEngine; 
using System.Collections;


public class AIControls : EntityCollider { 

	public float moveSpeed = 1;

	protected override Vector2 getNewMoveVector() {

		return new Vector2(0, 0);//TODO
	}
	
	protected override bool canRun() {
		return true;
	}
	
	protected override CharacterAction getCurrentCharacterAction() {
		return new CharacterAction(false, 100);
	}
	
	protected override BaseCharacterState getNextState() {
		return BaseCharacterState.WAIT;
	}

}

