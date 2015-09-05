using UnityEngine; 
using System.Collections;
using Level;
using InControl;


public class PlayerControls : EntityCollider { 

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
	private readonly KeyCode[] KEYS_ACTION = new KeyCode[] {
		KeyCode.Space,
		KeyCode.Return,
		KeyCode.KeypadEnter
	};
	private readonly string[] BUTTONS_ACTION = new string[] {
		"Action1"
	};


	public float moveSpeed = 1;

	public void setInitialPosition(NodePosition nodePosition, NodeDirection nodeDirection) {

		if(nodePosition != null) {
			setMapPosition(nodePosition.x, nodePosition.y);
		} else {
			setMapPosition(0, 0);
		}

		if(nodeDirection != null) {
			setDirection(nodeDirection.value);
		} else {
			setDirection(NodeDirection.Direction.UP);
		}
	}
	
	protected override void FixedUpdate() {

		base.FixedUpdate();

		if(isPaused) {
			return;
		}

		if(isAnyKeyPressed(KEYS_ACTION, false) || isAnyButtonPressed(BUTTONS_ACTION, false)) {
			//execute current action
			bool done = PlayerActionsManager.Instance.executeShownAction();
			if(!done) {
				//hide current message
				MessageDisplayer.Instance.displayNextMessage();
			}
		}

	}


	protected override Vector2 getNewMoveVector() {

		if(!isControlsEnabled) {
			return new Vector2();
		}

		float angleDegrees = 0;
		bool isPressingAnyDirection = false;

		// analogic directions
		if(!isPressingAnyDirection) {
			
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
				
				isPressingAnyDirection = true;
				
				angleDegrees = vectorToAngle(dx, dy) + 90;
			}
		}

		//keyboard
		if(!isPressingAnyDirection) {

			int dx = 0;
			int dy = 0;

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

				isPressingAnyDirection = true;

				angleDegrees = vectorToAngle(dx, dy) + 90;
			}

		}


		if(!isPressingAnyDirection) {
			return Vector2.zero;
		}

		float x = moveSpeed * Mathf.Cos(angleDegrees * Mathf.Deg2Rad);
		float y = moveSpeed * Mathf.Sin(angleDegrees * Mathf.Deg2Rad);

		return new Vector2(x, y);

	}
	
	
	private bool isKeyPressed(KeyCode key, bool longPress) {
		if(!isControlsEnabled) {
			return false;
		}
		if(longPress) {
			return Input.GetKey(key);
		}
		return Input.GetKeyDown(key);
	}
	
	private bool isAnyKeyPressed(KeyCode[] keys, bool longPress) {

		foreach (KeyCode k in keys) {
			if(isKeyPressed(k, longPress)) {
				return true;
			}
		}

		return false;
	}
	
	private bool isAllKeysPressed(KeyCode[] keys, bool longPress) {
		
		foreach (KeyCode k in keys) {
			if(!isKeyPressed(k, longPress)) {
				return false;
			}
		}
		
		return true;
	}
	
	private bool isButtonPressed(InputControl ic, bool longPress) {
		if(!isControlsEnabled) {
			return false;
		}
		if(longPress) {
			return (ic.IsButton && ic.IsPressed);
		}
		return (ic.IsButton && ic.IsPressed && ic.WasPressed);
	}
	
	private bool isButtonPressed(string inputControlName, bool longPress) {
		return isButtonPressed(InputManager.ActiveDevice.GetControlByName(inputControlName), longPress);
	}

	private bool isAnyButtonPressed(string[] inputControlNames, bool longPress) {
		
		foreach (string name in inputControlNames) {
			if(isButtonPressed(name, longPress)) {
				return true;
			}
		}
		
		return false;
	}
	
	private bool isAllButtonsPressed(string[] inputControlNames, bool longPress) {
		
		foreach (string name in inputControlNames) {
			if(!isButtonPressed(name, longPress)) {
				return false;
			}
		}
		
		return true;
	}


	protected override CharacterAnimation getCurrentCharacterAnimation() {

		string textureName = "Character.Rat.";

		switch(currentState) {

		case CharacterState.WALK:
			return new CharacterAnimation(
				false, 
				textureName + "Walk.png",
				new CharacterAnimationKey(0.15f),
				new CharacterAnimationKey(0.15f));
		}

		//wait
		return new CharacterAnimation(
			false, 
			textureName + "Wait.png",
			new CharacterAnimationKey(1.4f),
			new CharacterAnimationKey(0.6f));

	}
	
	protected override CharacterState getNextState() {
		
		if(currentState == CharacterState.WALK) {
			return CharacterState.WALK;
		}

		return CharacterState.WAIT;
	}

}

