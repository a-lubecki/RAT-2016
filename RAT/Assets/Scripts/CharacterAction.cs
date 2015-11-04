using System;

public class CharacterAction {
	
	public delegate void DelegateCharacterAction(CharacterAction characterAction);

	public readonly bool isBlocking;
	public readonly float durationSec;
	public readonly DelegateCharacterAction delegateAction;
	public readonly DelegateCharacterAction delegateOnFinish;

	public CharacterAction(bool isBlocking, float durationSec) : this(isBlocking, durationSec, null, null) {
	}

	public CharacterAction(bool isBlocking, float durationSec, DelegateCharacterAction delegateAction) : this(isBlocking, durationSec, delegateAction, null) {
	}
	
	public CharacterAction(bool isBlocking, float durationSec, DelegateCharacterAction delegateAction, DelegateCharacterAction delegateOnFinish) {
		
		if(durationSec <= 0) {
			throw new System.ArgumentException();
		}
		
		this.isBlocking = isBlocking;
		this.durationSec = durationSec;
		this.delegateAction = delegateAction;
		this.delegateOnFinish = delegateOnFinish;
	}

}

