using UnityEngine; 
using System.Collections; 
using Level;

public abstract class EntityCollider : MonoBehaviour {

	public EntityRenderer entityRenderer;

	public float angleDegrees { get; protected set; }

	public bool isMoving { get; private set; }
	public bool isRunning { get; protected set; }
	private Coroutine coroutineRun;
	
	protected bool isControlsEnabled { get; private set; }
	protected bool isControlsEnabledWhileAnimating { get; private set; }

	protected bool isPaused { get; private set; }

	public BaseCharacterState currentState { get; private set; }
	public CharacterDirection currentDirection { get; private set; }

	private Coroutine coroutineStateAnimation;


	protected virtual void Start() {

		isControlsEnabled = true;
		isControlsEnabledWhileAnimating = true;

		changeState(BaseCharacterState.WAIT);
	}

	public void setInitialPosition(int x, int y, int angleDegrees) {
		
		GetComponent<Transform>().position = new Vector2(x, y);
		
		setAngleDegrees(angleDegrees);

		updateStateWithDirection();
	}

	public void setMapPosition(int xGeneration, int yGeneration) {

		GetComponent<Transform>().position = GameHelper.Instance.newPositionOnMap(xGeneration, yGeneration);
	}

	public void setDirection(NodeDirection.Direction direction) {

		if(direction == NodeDirection.Direction.UP) {
			angleDegrees = 0;
		} else if(direction == NodeDirection.Direction.RIGHT) {
			angleDegrees = 90;
		} else if(direction == NodeDirection.Direction.DOWN) {
			angleDegrees = 180;
		} else if(direction == NodeDirection.Direction.LEFT) {
			angleDegrees = 270;
		}
		
		updateStateWithDirection();
 	}

	private void setAngleDegrees(float angleDegrees) {

		this.angleDegrees = angleDegrees % 360;

		if(this.angleDegrees < 0) {
			this.angleDegrees += 360;
		}
	}

	protected virtual void FixedUpdate() {

		if(isPaused) {
			return;
		}

		Vector2 newVector = getNewMoveVector();

		//update infos
		float dx = newVector.x;
		float dy = newVector.y;

		bool wasMoving = isMoving;

		isMoving = (dx != 0 || dy != 0);

		if(isMoving) {

			setAngleDegrees(vectorToAngle(newVector.x, newVector.y));

			Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

			//update transform with int vector to move with the grid
			rigidBody.MovePosition(
				new Vector2(
					rigidBody.position.x + dx * Time.deltaTime, 
					rigidBody.position.y + dy * Time.deltaTime
				)
			);

			updateStateWithDirection();

			//trigger walk/run state
			if(!wasMoving && currentState != BaseCharacterState.WALK) {
				changeState(BaseCharacterState.WALK);
			} else if(isRunning && currentState != BaseCharacterState.RUN) {
				changeState(BaseCharacterState.RUN);
			}

		}

	}

	
	protected void startRunningAfterDelay(float delay) {
		
		if(isRunning) {
			return;
		}
		
		if(coroutineRun == null) {
			coroutineRun = StartCoroutine(runAfterDelay(delay));
		}
	}
	
	protected void stopRunning() {
		
		if(coroutineRun != null) {
			StopCoroutine(coroutineRun);
			coroutineRun = null;
		}
		
		isRunning = false;

		didStopRunning();
	}
	
	private IEnumerator runAfterDelay(float delay) {
		
		if(delay > 0) {
			yield return new WaitForSeconds(delay);
		}
		
		if(!canRun()) {
			yield break;
		}
		
		isRunning = true;

		didStartRunning();
	}
	
	protected abstract bool canRun();

	protected virtual void didStartRunning() {
		//override this
	}

	protected virtual void didStopRunning() {
		//override this
	}


	private void updateStateWithDirection() {
		
		CharacterDirection tempDirection = getCharacterDirection(55);
		if(currentDirection != tempDirection) {
			
			currentDirection = tempDirection;
			
			//update state
			updateState(currentState);
		}
	}

	private CharacterDirection getCharacterDirection(int halfAngle) {

		if(currentDirection == CharacterDirection.RIGHT ||
		   currentDirection == CharacterDirection.LEFT) {
			
			if(isCharacterDirectionRight(angleDegrees, halfAngle)) {
				return CharacterDirection.RIGHT;
			} 
			if(isCharacterDirectionLeft(angleDegrees, halfAngle)) {
				return CharacterDirection.LEFT;
			} 
			if(isCharacterDirectionUp(angleDegrees, halfAngle)) {
				return CharacterDirection.UP;
			} 
			if(isCharacterDirectionDown(angleDegrees, halfAngle)) {
				return CharacterDirection.DOWN;
			}
		}

		if(currentDirection == CharacterDirection.UP ||
		   currentDirection == CharacterDirection.DOWN) {
			 
			if(isCharacterDirectionUp(angleDegrees, halfAngle)) {
				return CharacterDirection.UP;
			} 
			if(isCharacterDirectionDown(angleDegrees, halfAngle)) {
				return CharacterDirection.DOWN;
			} 
			if(isCharacterDirectionRight(angleDegrees, halfAngle)) {
				return CharacterDirection.RIGHT;
			} 
			if(isCharacterDirectionLeft(angleDegrees, halfAngle)) {
				return CharacterDirection.LEFT;
			}
		}

		return CharacterDirection.DOWN;
	}
	
	private static bool isCharacterDirectionUp(float angle, int halfAngle) {
		return (360 - halfAngle <= angle || angle <= halfAngle);
	}
	private static bool isCharacterDirectionRight(float angle, int halfAngle) {
		return (90 - halfAngle <= angle && angle <= 90 + halfAngle);
	}
	private static bool isCharacterDirectionDown(float angle, int halfAngle) {
		return (180 - halfAngle <= angle && angle <= 180 + halfAngle);
	}
	private static bool isCharacterDirectionLeft(float angle, int halfAngle) {
		return (270 - halfAngle <= angle && angle <= 270 + halfAngle);
	}
	
	public static float directionToAngle(CharacterDirection direction) {
		
		if(direction == CharacterDirection.RIGHT) {
			return 90;
		} 
		if(direction == CharacterDirection.LEFT) {
			return -90;
		} 
		if(direction == CharacterDirection.UP) {
			return 0;
		}
		return 180;
	}

		
	protected void OnApplicationFocus(bool focusStatus) {
		this.isPaused = !focusStatus;
	}


	protected abstract Vector2 getNewMoveVector();

	public static float vectorToAngle(float x, float y) {
		
		if(y == 0) {
			return (x > 0) ? 90 : -90;
		}
		
		return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
	}

	public static Vector2 angleToVector(float angleDegrees, int force) {

		float angleRad = angleDegrees * Mathf.Deg2Rad;

		return new Vector2(Mathf.Sin(angleRad) * force, Mathf.Cos(angleRad) * force);
	}

	protected void changeState(BaseCharacterState state) {
		
		if(state == BaseCharacterState.UNDEFINED) {
			return;
		}

		if(state == currentState) {
			return;
		}

		updateState(state);
	}

	protected void updateState(BaseCharacterState state) {

		if(coroutineStateAnimation != null) {
			StopCoroutine(coroutineStateAnimation);
		}

		currentState = state;
		
		if(state != BaseCharacterState.UNDEFINED) {
			coroutineStateAnimation = StartCoroutine(animateCharacter());
		}
	
	}

	private IEnumerator animateCharacter() {

		CharacterAction characterAction = getCurrentCharacterAction();

		if(characterAction.isBlocking) {
			isControlsEnabledWhileAnimating = false;
		}

		//call action
		if(characterAction.delegateAction != null) {
			characterAction.delegateAction(characterAction);
		}

		//render
		entityRenderer.animate(currentState, characterAction);

		yield return new WaitForSeconds(characterAction.durationSec);
		
		//call onfinish action
		if(characterAction.delegateOnFinish != null) {
			characterAction.delegateOnFinish(characterAction);
		}

		isControlsEnabledWhileAnimating = true;

		updateState(getNextState());
	}
	
	protected abstract CharacterAction getCurrentCharacterAction();

	protected abstract BaseCharacterState getNextState();
	
	public void enableControls() {
		isControlsEnabled = true;
	}

	public void disableControls() {
		isControlsEnabled = false;
	}

}

