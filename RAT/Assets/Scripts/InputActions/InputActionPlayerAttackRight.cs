using System;
using UnityEngine;

public class InputActionPlayerAttackRight : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Keypad3,
			KeyCode.P
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] { 
			"RightBumper",
			"RightTrigger"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		GameHelper.Instance.getPlayerControls().tryRightAttack();

		return true;
	}

}

