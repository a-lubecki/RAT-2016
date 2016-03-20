using System;
using UnityEngine;
using InControl;

public class InputActionPlayerMove : AbstractInputAction {
	
	private readonly KeyCode[] KEYS_DIRECTION_RIGHT = new KeyCode[] {
		KeyCode.D
	};
	private readonly KeyCode[] KEYS_DIRECTION_LEFT = new KeyCode[] {
		KeyCode.Q
	};
	private readonly KeyCode[] KEYS_DIRECTION_UP = new KeyCode[] {
		KeyCode.Z
	};
	private readonly KeyCode[] KEYS_DIRECTION_DOWN = new KeyCode[] {
		KeyCode.S
	};


	public float angleDegrees { get; private set; }
	public float analogicFactor { get; private set; }

	public override KeyCode[] getDefaultActionKeys() {
		return null;
	}

	public override string[] getDefaultActionButtons() {
		return null;
	}


	public override bool isActionDone() {
		
		PlayerBehavior playerBehavior = GameHelper.Instance.findPlayerBehavior();
		if(!playerBehavior.isControlsEnabled || !playerBehavior.isControlsEnabledWhileAnimating) {
			return false;
		}

		analogicFactor = 0;
		angleDegrees = 0;

		
		// analogic directions
		InputDevice activeDevice = InputManager.ActiveDevice;
		
		float dx = - activeDevice.LeftStickX.Value;
		float dy = activeDevice.LeftStickY.Value;
		
		if(dx > 1) {
			dx = 1;
		}
		if(dy > 1) {
			dy = 1;
		}
		
		if(dx != 0 || dy != 0) {
			analogicFactor = Mathf.Sqrt(dx*dx + dy*dy);
			angleDegrees = Constants.vectorToAngle(dx, dy) + 90;
		}


		//keyboard
		if(analogicFactor <= 0) {
			
			if(isAnyKeyPressed(KEYS_DIRECTION_RIGHT, true)) {
				dx += -1;
			}
			if(isAnyKeyPressed(KEYS_DIRECTION_LEFT, true)) {
				dx += 1;
			}
			
			if(isAnyKeyPressed(KEYS_DIRECTION_UP, true)) {
				dy += 1;
			}
			if(isAnyKeyPressed(KEYS_DIRECTION_DOWN, true)) {
				dy += -1;
			}
			
			if(dx != 0 || dy != 0) {
				analogicFactor = 1;
				angleDegrees = Constants.vectorToAngle(dx, dy) + 90;
			}

		}
		
		return (analogicFactor > 0);
	}

	public override bool execute() {

		//do nothing
		return true;
	}
	

}

