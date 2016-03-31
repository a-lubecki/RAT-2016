using System;
using UnityEngine;

public class InputActionMenuCancel : AbstractInputActionMenu {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Escape 
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"Action2"
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
			GameHelper.Instance.getMenu().cancel();
		}

		return true;
	}

}

