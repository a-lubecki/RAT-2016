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
		KeyCode.Return,
		KeyCode.KeypadEnter
	};
	private readonly string[] BUTTONS_ACTION = new string[] {
		"Action1"
	};
	private readonly KeyCode[] KEYS_DASH = new KeyCode[] {
		KeyCode.Space
	};
	private readonly string[] BUTTONS_DASH = new string[] {
		"Action2"
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
	
	protected void Update() {

		if(isPaused) {
			return;
		}

		if(isAnyKeyPressed(KEYS_ACTION, false) || isAnyButtonPressed(BUTTONS_ACTION, false)) {

			bool hasMessage = MessageDisplayer.Instance.isShowingMessage();
			if(hasMessage) {
				//hide current message
				MessageDisplayer.Instance.displayNextMessage();
			} else {
				//execute current action
				PlayerActionsManager.Instance.executeShownAction();
			}

		}
		
		if(isAnyKeyPressed(KEYS_DASH, false) || isAnyButtonPressed(BUTTONS_DASH, false)) {

			Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
			if(player.stamina > 0) {
				//can't dash if no stamina
				dash();
			}
		}

	}


	protected override Vector2 getNewMoveVector() {

		if(!isControlsEnabled) {
			return new Vector2();
		}

		float angleDegrees = 0;
		float analogicFactor = 1;
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

				analogicFactor = Mathf.Sqrt(dx*dx + dy*dy);
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

		if(analogicFactor < 0) {
			analogicFactor = 0;
		} else if(analogicFactor > 1) {
			analogicFactor = 1;
		} 

		//change the 0 => 1 constant function to an exponential function
		analogicFactor = analogicFactor*analogicFactor;

		float x = analogicFactor * moveSpeed * Mathf.Cos(angleDegrees * Mathf.Deg2Rad);
		float y = analogicFactor * moveSpeed * Mathf.Sin(angleDegrees * Mathf.Deg2Rad);

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

		if(currentState == BaseCharacterState.WALK) {
			return new CharacterAnimation(
				false, 
				textureName + "Walk.png",
				new CharacterAnimationKey(0.15f),
				new CharacterAnimationKey(0.15f));
		}
		
		if(currentState == PlayerState.DASH) {
			return new CharacterAnimation(
				true, 
				textureName + "Wait.png",//TODO test
				new CharacterAnimationKey(0.001f),//TODO test
				new CharacterAnimationKey(0.5f));
		}

		//wait
		return new CharacterAnimation(
			false, 
			textureName + "Wait.png",
			new CharacterAnimationKey(1.4f),
			new CharacterAnimationKey(0.6f));

	}
	
	protected override BaseCharacterState getNextState() {

		if(currentState == BaseCharacterState.WALK && isMoving) {
			return BaseCharacterState.WALK;
		}

		return BaseCharacterState.WAIT;
	}


	protected void dash() {
		
		float angle;

		if(isMoving) {

			angle = angleDegrees;

		} else {
			
			//snap the angle using the character direction
			if(currentDirection == CharacterDirection.RIGHT) {
				angle = 90;
			} else if(currentDirection == CharacterDirection.LEFT) {
				angle = -90;
			} else if(currentDirection == CharacterDirection.UP) {
				angle = 0;
			} else {
				angle = 180;
			}

			//dash opposite
			angle += 180;

		}

		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		
		Vector2 newForce = angleToVector(angle, 50000);

		//update transform with int vector to move with the grid
		rigidBody.AddForce(
			new Vector2(
			newForce.x, 
			newForce.y
			)
		);

		//remove stamina
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		player.stamina -= 20;//TODO test value

		updateState(PlayerState.DASH);
		
	}

}

