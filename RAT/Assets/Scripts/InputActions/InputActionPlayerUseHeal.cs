using System;
using UnityEngine;

public class InputActionPlayerUseHeal : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.Keypad9,
			KeyCode.E
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"Action3"
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

