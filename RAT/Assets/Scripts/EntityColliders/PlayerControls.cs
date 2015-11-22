using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	private readonly KeyCode[] KEYS_RUN = new KeyCode[] {//only for keyboard, not controller or mobile
		KeyCode.RightAlt,
		KeyCode.LeftControl,
		KeyCode.RightControl
	};
	private readonly string[] BUTTONS_RUN = new string[] {
		"Action2"
	};
	private readonly KeyCode[] KEYS_DASH = new KeyCode[] {
		KeyCode.Space
	};
	private readonly string[] BUTTONS_DASH = new string[] {
		"Action2"
	};
	private readonly KeyCode[] KEYS_RIGHT_ATTACK = new KeyCode[] {
		KeyCode.Keypad3,
		KeyCode.P
	};
	private readonly string[] BUTTONS_RIGHT_ATTACK = new string[] {
		"RightBumper",
		"RightTrigger"
	};
	private readonly KeyCode[] KEYS_LEFT_ATTACK = new KeyCode[] {
		KeyCode.Keypad1,
		KeyCode.O
	};
	private readonly string[] BUTTONS_LEFT_ATTACK = new string[] {
		"RightBumper",
		"RightTrigger"
	};
	private readonly KeyCode[] KEYS_USE_OBJECT = new KeyCode[] {
		KeyCode.Keypad7,
		KeyCode.A
	};
	private readonly string[] BUTTONS_USE_OBJECT = new string[] {
		"Action4"
	};
	private readonly KeyCode[] KEYS_USE_HEAL = new KeyCode[] {
		KeyCode.Keypad9,
		KeyCode.E
	};
	private readonly string[] BUTTONS_USE_HEAL = new string[] {
		"Action3"
	};
	
	private readonly KeyCode[] KEYS_PREVIOUS_OBJECT = new KeyCode[] {
		KeyCode.Keypad4,
		KeyCode.Alpha1
	};
	private readonly string[] BUTTONS_PREVIOUS_OBJECT = new string[] {
		"DPadLeft"
	};
	private readonly KeyCode[] KEYS_NEXT_OBJECT = new KeyCode[] {
		KeyCode.Keypad6,
		KeyCode.Alpha2
	};
	private readonly string[] BUTTONS_NEXT_OBJECT = new string[] {
		"DPadRight"
	};
	private readonly KeyCode[] KEYS_PREVIOUS_HEAL = new KeyCode[] {
		KeyCode.Keypad8,
		KeyCode.Alpha3
	};
	private readonly string[] BUTTONS_PREVIOUS_HEAL = new string[] {
		"DPadDown"
	};
	private readonly KeyCode[] KEYS_NEXT_HEAL = new KeyCode[] {
		KeyCode.Keypad2,
		KeyCode.Alpha4
	};
	private readonly string[] BUTTONS_NEXT_HEAL = new string[] {
		"DPadUp"
	};


	public float MOVE_SPEED = 1;
	
	private static readonly int MAX_BUTTON_LONG_PRESS_ITERATIONS = 30;
	private Dictionary<string, int> buttonPressIterations = new Dictionary<string, int>();

	private bool isPressingAnyDirection = false;

	private bool isRegainingStamina = false;
	private Coroutine coroutineRegainingStamina;

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

	protected override void Start() {
		base.Start();

		InvokeRepeating("manageStamina", Player.STAMINA_UPDATE_FREQUENCY_SEC, Player.STAMINA_UPDATE_FREQUENCY_SEC);

		//if coming from a save when the player has not full stamina, regain it
		startRegainingStaminaAfterDelay(1f);
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
			
			Player player = GameHelper.Instance.getPlayer();
			
			if(player.stamina > 0) {
				updateState(PlayerState.DASH);
			}

		}
		
		if(isAnyKeyPressed(KEYS_RIGHT_ATTACK, false) || isAnyButtonPressed(BUTTONS_RIGHT_ATTACK, false)) {

			Player player = GameHelper.Instance.getPlayer();
			
			if(player.stamina > 0) {
				updateState(PlayerState.SHORT_ATTACK);
			}

		}

		if(isAnyKeyPressed(KEYS_LEFT_ATTACK, false) || isAnyButtonPressed(BUTTONS_LEFT_ATTACK, false)) {
			
			Player player = GameHelper.Instance.getPlayer();
			
			if(player.stamina > 0) {
				updateState(PlayerState.SHORT_ATTACK);
			}

		}

	}
	
	protected void OnDisable() {

		stopRunning();
	}


	protected override Vector2 getNewMoveVector() {

		if(!isControlsEnabled || !isControlsEnabledWhileAnimating) {
			return new Vector2();
		}

		float angleDegrees = 0;
		float analogicFactor = 1;
		bool hasStartedRunning = false;
		
		isPressingAnyDirection = false;

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
				
				if(isAnyButtonPressed(BUTTONS_RUN, true)) {
					startRunningAfterDelay(0.4f);
					hasStartedRunning = true;
				}/* else if(analogicFactor >= 0.99) {
					startRunningAfterDelay(2f);
					hasStartedRunning = true;
				}*/
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

				if(isAnyKeyPressed(KEYS_RUN, true)) {
					startRunningAfterDelay(0.4f);
					hasStartedRunning = true;
				}
			}

		}


		if(!hasStartedRunning) {
			stopRunning();
		}

		if(!isPressingAnyDirection) {
			return Vector2.zero;
		}

		if(isRunning) {

			analogicFactor = 1.6f;

		} else {

			if(analogicFactor < 0.2) {
				analogicFactor = 0;
			} else if(analogicFactor > 1) {
				analogicFactor = 1;
			}
			
			//change the 0 => 1 constant function to an exponential function
			analogicFactor = analogicFactor*analogicFactor;
		}

		float x = analogicFactor * MOVE_SPEED * Mathf.Cos(angleDegrees * Mathf.Deg2Rad);
		float y = analogicFactor * MOVE_SPEED * Mathf.Sin(angleDegrees * Mathf.Deg2Rad);

		return new Vector2(x, y);

	}
	
	
	private bool isKeyPressed(KeyCode key, bool longPress) {

		if(!isControlsEnabled || !isControlsEnabledWhileAnimating) {
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

	private bool isButtonPressed(string inputControlName, bool longPress) {
		
		/*{
			InputControl ic = InputManager.ActiveDevice.GetControlByName(inputControlName);
			if(ic.Equals(InputManager.ActiveDevice.Action2) && 
			   (ic.IsPressed || ic.WasPressed || ic.WasReleased || ic.HasChanged || ic.LastState)) {
				Debug.Log(">>> IsPressed(" + ic.IsPressed + ") WasPressed(" + ic.WasPressed +
				          ") WasReleased(" + ic.WasReleased + ") LastValue(" + ic.LastValue + ") Value(" + ic.Value +
				          ") LastState(" + ic.LastState + ") State(" + ic.State +
				          ") HasChanged(" + ic.HasChanged);
			}
		}*/

		if(!isControlsEnabled || !isControlsEnabledWhileAnimating) {
			return false;
		}

		InputControl ic = InputManager.ActiveDevice.GetControlByName(inputControlName);
			
		if(!buttonPressIterations.ContainsKey(inputControlName)) {
			//register button if missing
			buttonPressIterations.Add(inputControlName, 0);
		}
		
		int iterationsCount = buttonPressIterations[inputControlName];

		if(!ic.State) {

			if(!ic.HasChanged) {
				//not pressing the button
				return false;
			}

			//user has just stopped pressing the button
			buttonPressIterations[inputControlName] = 0;

		} else {

			//user is still pressing the button
			buttonPressIterations[inputControlName]++;
		}

		
		if(longPress) {
			//long press only if there were too many iterations
			return (iterationsCount >= MAX_BUTTON_LONG_PRESS_ITERATIONS);
		}

		//check short press

		if(iterationsCount < MAX_BUTTON_LONG_PRESS_ITERATIONS && !ic.State) {
			//short press only if the max of iterations was not reached when releasing the button
			return true;
		}

		return false;
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
	
	protected override CharacterAction getCurrentCharacterAction() {
		
		if(currentState == BaseCharacterState.WALK) {
			return new CharacterAction(false, 0.4f);
		}
		
		if(currentState == BaseCharacterState.RUN) {
			return new CharacterAction(false, 0.2f);
		}
		
		if(currentState == PlayerState.DASH) {
			return new CharacterAction(true, 0.5f, delegate(CharacterAction action) {
				
				Player player = GameHelper.Instance.getPlayer();

				//remove stamina
				player.stamina -= Player.STAMINA_CONSUMPTION_DASH;
				
				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();

				float angle;
				if(isMoving) {
					angle = angleDegrees;
				} else {
					angle = directionToAngle(currentDirection);
					
					//dash opposite
					angle += 180;
				}

				StartCoroutine(dashAfterDelay(angle, 50000, 0.1f));

			}, delegate(CharacterAction action) {
				
				startRegainingStaminaAfterDelay(Player.DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);

			});
		}
		
		if(currentState == PlayerState.SHORT_ATTACK) {
			return new CharacterAction(true, 0.9f, delegate(CharacterAction action) {
				
				Player player = GameHelper.Instance.getPlayer();

				//remove stamina
				player.stamina -= Player.STAMINA_CONSUMPTION_SHORT_ATTACK;
				
				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();
				
				float angle;				
				if(isMoving) {
					angle = angleDegrees;
				} else {
					angle = directionToAngle(currentDirection);
				}
				StartCoroutine(dashAfterDelay(angle, 20000, 0.2f));
				
			}, delegate(CharacterAction action) {
				
				startRegainingStaminaAfterDelay(Player.DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);
				
			});
		}

		if(currentState == PlayerState.HEAVY_ATTACK) {
			return new CharacterAction(true, 1.5f, delegate(CharacterAction action) {
				
				Player player = GameHelper.Instance.getPlayer();

				//remove stamina
				player.stamina -= Player.STAMINA_CONSUMPTION_HEAVY_ATTACK;
				
				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();
				
				float angle;				
				if(isMoving) {
					angle = angleDegrees;
				} else {
					angle = directionToAngle(currentDirection);
				}
				StartCoroutine(dashAfterDelay(angle, 30000, 0.5f));
				
			}, delegate(CharacterAction action) {
				
				startRegainingStaminaAfterDelay(Player.DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);
				
			});
		}
		
		if(currentState == PlayerState.DEFEND) {
			return new CharacterAction(false, 1);
		}

		//wait
		return new CharacterAction(false, 2);

	}

	protected override BaseCharacterState getNextState() {
		
		if(isMoving) { 
			if(currentState == BaseCharacterState.WALK) {
				return BaseCharacterState.WALK;
			}
			if(currentState == BaseCharacterState.RUN) {
				return BaseCharacterState.WALK;
			}
		}

		return BaseCharacterState.WAIT;
	}



	protected override bool canRun() {

		if(!isPressingAnyDirection) {
			//no need to run if no direction pressed
			return false;
		}
		
		Player player = GameHelper.Instance.getPlayer();
		if(player.stamina <= 0) {
			//can't run if no stamina
			return false;
		}

		return true;
	}
	
	protected override void didStartRunning() {
		stopRegainingStamina();
	}

	protected override void didStopRunning() {
		startRegainingStaminaAfterDelay(1f);
	}
	
	protected IEnumerator dashAfterDelay(float angle, int force, float delay) {
		
		yield return new WaitForSeconds(delay);
		
		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		
		Vector2 newForce = angleToVector(angle, force);
		
		//update transform with int vector to move with the grid
		rigidBody.AddForce(
			new Vector2(
			newForce.x, 
			newForce.y
			)
		);
		
	}

	protected void startRegainingStaminaAfterDelay(float delay) {
		
		if(isRegainingStamina) {
			return;
		}

		if(!isActiveAndEnabled) {
			return;
		}
		
		if(coroutineRegainingStamina == null) {
			coroutineRegainingStamina = StartCoroutine(regainStaminaAfterDelay(1f));
		}
	}
	
	protected void stopRegainingStamina() {
		
		if(coroutineRegainingStamina != null) {
			StopCoroutine(coroutineRegainingStamina);
			coroutineRegainingStamina = null;
		}
		
		isRegainingStamina = false;
	}
	
	private IEnumerator regainStaminaAfterDelay(float delay) {
		
		if(delay > 0) {
			yield return new WaitForSeconds(delay);
		}
		
		isRegainingStamina = true;
	}

	private void manageStamina() {

		Player player = GameHelper.Instance.getPlayer();

		if(player.isDead()) {
			stopRegainingStamina();
			return;
		}

		if(isRunning) {

			player.stamina -= Player.STAMINA_CONSUMPTION_RUN;

			//can't run any more
			if(player.stamina <= 0) {
				stopRunning();
			}

		} else if(isRegainingStamina) {

			player.stamina += Player.STAMINA_REGAIN_REST;

			if(player.stamina >= player.maxStamina) {
				stopRegainingStamina();
			}
		}

	}

}

