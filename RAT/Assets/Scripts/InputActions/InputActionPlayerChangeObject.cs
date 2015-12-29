using System;
using UnityEngine;

public class InputActionPlayerChangeObject : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.Keypad2,
			KeyCode.Alpha4,
			KeyCode.F
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
		
		//TODO
		
		return true;
	}

}

