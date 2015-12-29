using System;
using UnityEngine;

public class InputActionPlayerChangeHeal : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.Keypad8,
			KeyCode.Alpha3,
			KeyCode.R
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

		//TODO

		return true;
	}

}

