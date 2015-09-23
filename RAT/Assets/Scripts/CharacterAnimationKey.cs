using System;

public class CharacterAnimationKey {
	
	public readonly float percentage;
	public readonly int imagePos;

	public CharacterAnimationKey(float percentage, int imagePos) {
		
		if(percentage < 0) {
			throw new System.ArgumentException();
		}
		if(percentage > 1) {
			throw new System.ArgumentException();
		}
		if(imagePos < 0) {
			throw new System.ArgumentException();
		}
		
		this.percentage = percentage;
		this.imagePos = imagePos;
	}

}

