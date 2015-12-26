using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;

public class DefaultNpcRenderer : EntityRenderer {


	protected override CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState) {
		
		string textureName = "Enemy.Insect.Wait";
		
		//wait
		return new CharacterAnimation(
			textureName,
			new CharacterAnimationKey(0, 0));
	}

}
