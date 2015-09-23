using System;

public class CharacterAction {
	
	public readonly bool isBlocking;
	public readonly float durationSec;

	public CharacterAction(bool isBlocking, float durationSec) {

		if(durationSec <= 0) {
			throw new System.ArgumentException();
		}

		this.isBlocking = isBlocking;
		this.durationSec = durationSec;
	}

}

