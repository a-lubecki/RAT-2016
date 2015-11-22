using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;

public class PlayerRenderer : EntityRenderer {


	protected override CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState) {

		string textureName = "Character.Rat.";
		
		if(characterState == BaseCharacterState.WALK) {
			return new CharacterAnimation(
				textureName + "Walk.png",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}
		
		if(characterState == BaseCharacterState.RUN) {
			return new CharacterAnimation(
				textureName + "Walk.png",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}
		
		if(characterState == PlayerState.DASH) {
			return new CharacterAnimation(
				textureName + "Walk.png",
				new CharacterAnimationKey(0, 1));
		}
		
		if(characterState == PlayerState.SHORT_ATTACK) {
			return new CharacterAnimation(
				textureName + "Attack.png",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.2f, 1));
		}
		
		if(characterState == PlayerState.HEAVY_ATTACK) {
			return new CharacterAnimation(
				textureName + "Attack.png",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}

		if(characterState == PlayerState.DEFEND) {
			return new CharacterAnimation(
				textureName + "Wait.png",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}

		//wait
		return new CharacterAnimation(
			textureName + "Wait.png",
			new CharacterAnimationKey(0, 0),
			new CharacterAnimationKey(0.7f, 1));
	}

}
