using System;
using UnityEngine;

public class InputActionPlayerChangeWeaponLeft : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.Keypad4,
			KeyCode.Alpha1,
			KeyCode.C
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
		
		//TODO
		
		return true;
	}

}

