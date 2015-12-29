using System;
using UnityEngine;

public class InputActionMenuNavigateUp : AbstractInputActionMenu {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.UpArrow
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"DPadUp"
		};
	}
	
	public override bool execute() {

		if(!base.execute()) {
			return false;
		}

		GameHelper.Instance.getMenu().navigateUp();

		return true;
	}

}

