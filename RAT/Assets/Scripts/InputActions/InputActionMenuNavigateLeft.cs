using System;
using UnityEngine;

public class InputActionMenuNavigateLeft : AbstractInputActionMenu {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.LeftArrow
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"DPadLeft"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		GameHelper.Instance.getMenu().navigateLeft();

		return true;
	}

}

