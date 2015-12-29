using System;
using UnityEngine;

public class InputActionMenuNavigateRight : AbstractInputActionMenu {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.RightArrow
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"DPadRight"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		GameHelper.Instance.getMenu().navigateRight();
		
		return true;
	}

}

