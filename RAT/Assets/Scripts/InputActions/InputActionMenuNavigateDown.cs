using System;
using UnityEngine;

public class InputActionMenuNavigateDown : AbstractInputActionMenu {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.DownArrow
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"DPadDown"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		GameHelper.Instance.getMenu().navigateDown();
		
		return true;
	}

}

