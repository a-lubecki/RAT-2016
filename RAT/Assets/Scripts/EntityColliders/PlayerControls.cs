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

		InvokeRepeating("manageStamina", Player.STAMINA_UPDATE_FREQUENCY_S, Player.STAMINA_UPDATE_FREQUENCY_S);

		//if coming from a save when the player has not full stamina, regain it
		startRegainingStaminaAfterDelay();
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
	
	protected void OnDisable() {

		stopRunning();
	}


	protected override Vector2 getNewMoveVector() {

		if(!isControlsEnabled) {
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

		if(!isControlsEnabled) {
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
			return new CharacterAction(true, 0.5f);
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
		player.stamina -= Player.STAMINA_CONSUMPTION_DASH;

		//after a dash, the player can't continue the same running
		stopRunning();
		stopRegainingStamina();

		updateState(PlayerState.DASH);

		startRegainingStaminaAfterDelay();

	}

	protected override bool canRun() {

		if(!isPressingAnyDirection) {
			//no need to run if no direction pressed
			return false;
		}
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
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
		startRegainingStaminaAfterDelay();
	}

	protected void startRegainingStaminaAfterDelay() {
		
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

		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();

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

