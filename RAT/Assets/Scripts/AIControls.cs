using UnityEngine; 
using System.Collections;


public class AIControls : EntityCollider { 

	public float moveSpeed = 1;

	protected override Vector2 getNewMoveVector() {

		return new Vector2(0, 0);//TODO
	}
	
	protected override CharacterAnimation getCurrentCharacterAnimation() {
		
		string textureName = "Enemy.Insect.Wait.png";

		//wait
		return new CharacterAnimation(
			false, 
			textureName,
			new CharacterAnimationKey(100f));
		
	}
	
	protected override CharacterState getNextState() {
		return CharacterState.WAIT;
	}

}

