using System;
using UnityEngine;

public class InputActionPlayerDash : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Space 
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] { 
			"Action2"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}
		
		GameHelper.Instance.getPlayer().tryDash();
		
		return true;
	}

}

