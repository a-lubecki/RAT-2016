using UnityEngine; 
using System.Collections; 
using Level;

public abstract class EntityCollider : MonoBehaviour {

	public EntityRenderer entityRenderer;

	public float angleDegrees { get; protected set; }
	public bool isMoving { get; private set; }

	
	protected bool isControlsEnabled { get; private set; }

	protected bool isPaused { get; private set; }

	public CharacterState currentState { get; private set; }
	public CharacterDirection currentDirection { get; private set; }
	
	public CharacterAnimation currentCharacterAnimation { get; private set; }
	public int currentCharacterAnimationKey { get; private set; }

	private Coroutine coroutineStateAnimation;

	void Start() {

		isControlsEnabled = true;

		setState(CharacterState.WAIT);
	}

	public void setInitialPosition(int x, int y, int angleDegrees) {
		
		GetComponent<Transform>().position = new Vector2(x, y);
		
		this.angleDegrees = angleDegrees;

		updateState();
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
			angleDegrees = -90;
		}
		
		updateState();
 	}

	void FixedUpdate() {

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

			angleDegrees = vectorToAngle(newVector.x, newVector.y);

			Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

			//update transform with int vector to move with the grid
			rigidBody.MovePosition(
				new Vector2(
					rigidBody.position.x + dx * Time.deltaTime, 
					rigidBody.position.y + dy * Time.deltaTime
				)
			);
			
			angleDegrees = (angleDegrees + 360) % 360;

			updateState();
		}

		//update state
		if(!wasMoving && isMoving && currentState != CharacterState.WALK){
			setState(CharacterState.WALK);
		}
		if(wasMoving && !isMoving && currentState != CharacterState.WAIT){
			setState(CharacterState.WAIT);
		}

	}

	private void updateState() {
		
		CharacterDirection tempDirection = getCharacterDirection(55);
		if(currentDirection != tempDirection) {
			
			currentDirection = tempDirection;
			
			//update state
			setState(currentState);
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


	private void setState(CharacterState state) {

		if(state == CharacterState.UNDEFINED) {
			return;
		}

		if(coroutineStateAnimation != null) {
			StopCoroutine(coroutineStateAnimation);
		}

		//Debug.Log(">>> STATE " + currentState + " => " + state);

		currentState = state;

		coroutineStateAnimation = StartCoroutine(animateCharacter());

	}

	private IEnumerator animateCharacter() {

		currentCharacterAnimation = getCurrentCharacterAnimation();

		if(currentCharacterAnimation.isBlocking) {
			isControlsEnabled = false;
		}

		currentCharacterAnimationKey = 0;
		foreach(CharacterAnimationKey key in currentCharacterAnimation.keys) {

			entityRenderer.updateSprite();

			yield return new WaitForSeconds(key.duration);

			currentCharacterAnimationKey++;
		}

		currentCharacterAnimation = null;

		isControlsEnabled = true;

		setState(getNextState());
	}
	
	protected abstract CharacterState getNextState();

	protected abstract CharacterAnimation getCurrentCharacterAnimation();

}

