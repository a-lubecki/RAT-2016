using System;
using UnityEngine;

public class InputActionPlayerChangeWeaponRight : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.Keypad6,
			KeyCode.Alpha2,
			KeyCode.V
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
		
		//TODO
		
		return true;
	}

}

