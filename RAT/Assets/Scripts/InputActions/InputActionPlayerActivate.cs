using System;
using UnityEngine;

public class InputActionPlayerActivate : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Return,
			KeyCode.KeypadEnter 
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] {
			"Action1"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		bool hasMessage = MessageDisplayer.Instance.isShowingMessage();
		if(hasMessage) {
			//hide current message
			MessageDisplayer.Instance.displayNextMessage();
		} else {
			//execute current action
			PlayerActionsManager.Instance.executeShownAction();
		}

		return true;
	}

}

