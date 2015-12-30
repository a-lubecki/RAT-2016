using System;
using UnityEngine;
using InControl;

public class InputActionPlayerMove : AbstractInputAction {
	
	private readonly KeyCode[] KEYS_DIRECTION_RIGHT = new KeyCode[] {
		KeyCode.RightArrow,
		KeyCode.D
	};
	private readonly KeyCode[] KEYS_DIRECTION_LEFT = new KeyCode[] {
		KeyCode.LeftArrow,
		KeyCode.Q
	};
	private readonly KeyCode[] KEYS_DIRECTION_UP = new KeyCode[] {
		KeyCode.UpArrow,
		KeyCode.Z
	};
	private readonly KeyCode[] KEYS_DIRECTION_DOWN = new KeyCode[] {
		KeyCode.DownArrow,
		KeyCode.S
	};


	private float _angleDegrees;
	public float angleDegrees {
		get {
			return _angleDegrees;
		}
	}

	private float _analogicFactor;
	public float analogicFactor {
		get {
			return _analogicFactor;
		}
		private set {
			_analogicFactor = value;
		}
	}

	public override KeyCode[] getDefaultActionKeys() {
		return null;
	}

	public override string[] getDefaultActionButtons() {
		return null;
	}


	public override bool isActionDone() {
		
		PlayerCollider playerController = GameHelper.Instance.getPlayerControls();
		if(!playerController.isControlsEnabled || !playerController.isControlsEnabledWhileAnimating) {
			return false;
		}

		_analogicFactor = 0;
		_angleDegrees = 0;

		
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
			_analogicFactor = Mathf.Sqrt(dx*dx + dy*dy);
			_angleDegrees = Constants.vectorToAngle(dx, dy) + 90;
		}


		//keyboard
		if(_analogicFactor <= 0) {
			
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
				_analogicFactor = 1;
				_angleDegrees = Constants.vectorToAngle(dx, dy) + 90;
			}

		}
		
		return (_analogicFactor > 0);
	}

	public override bool execute() {

		//do nothing
		return true;
	}
	

}

