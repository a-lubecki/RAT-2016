using System;
using UnityEngine;

public class InputActionPlayerRun : AbstractInputActionPlayer {

	private bool _isRunning;
	public bool isRunning {
		get {
			return _isRunning;
		}
	}

	public override bool areActionKeysLongPressed() {
		return true;
	}
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.RightAlt,
			KeyCode.LeftControl,
			KeyCode.RightControl 
		};
	}
	
	public override bool areActionButtonsLongPressed() {
		return true;
	}
	public override string[] getDefaultActionButtons() {
		return new string[] { 
			"Action2"
		};
	}
	
	public override bool isActionDone() {

		_isRunning = false;

		return base.isActionDone();
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
			return false;
		}

		_isRunning = true;

		return true;
	}

}

