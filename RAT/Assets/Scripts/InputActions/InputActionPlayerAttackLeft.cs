using System;
using UnityEngine;

public class InputActionPlayerAttackLeft : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Keypad1,
			KeyCode.O
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] { 
			"LeftBumper",
			"LeftTrigger"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		GameHelper.Instance.getPlayerControls().tryLeftAttack();

		return true;
	}

}

