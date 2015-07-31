using System;

public class CharacterAnimationKey {

	public readonly float duration;

	public CharacterAnimationKey(float duration) {

		if(duration <= 0) {
			throw new System.ArgumentException();
		}

		this.duration = duration;
	}

}

