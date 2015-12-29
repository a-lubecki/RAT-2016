using System;
using UnityEngine;

public class InputActionPlayerUseObject : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] {
			KeyCode.Keypad7,
			KeyCode.A
		};
	}
	
	public override string[] getDefaultActionButtons() {
		return new string[] {
			"Action4"
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

