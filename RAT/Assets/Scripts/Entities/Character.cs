using UnityEngine;
using System.Collections;
using Node;

public abstract class Character : MonoBehaviour {
	
	public CharacterRenderer characterRenderer;
	
	public float angleDegrees { get; protected set; }
	
	public bool isMoving { get; private set; }
	public bool isRunning { get; protected set; }
	private Coroutine coroutineRun;
	
	public bool isControlsEnabled { get; private set; }
	public bool isControlsEnabledWhileAnimating { get; private set; }
	
	public BaseCharacterState currentState { get; private set; }
	public CharacterDirection currentDirection { get; private set; }
	
	private Coroutine coroutineStateAnimation;
	

	private int _maxLife;
	public int maxLife { 
		get {
			return _maxLife;
		}
		set {
			if(value <= 0) {
				_maxLife = 0;
			} else {
				_maxLife = value;
			}
		}
	}
	
	private int _life;
	public int life { 
		get {
			return _life;
		}
		set {
			if(value <= 0) {
				_life = 0;
			} else if(value > _maxLife) {
				_life = _maxLife;
			} else {
				_life = value;
			}
		}
	}

	private bool isTemporaryInvulnerable = false;

	public bool isDead() {
		return (life <= 0);
	}

	public void takeDamages(int damages) {

		if(life <= 0) {
			//already dead
			return;
		}

		if(damages == 0) {
			//not a heal or a damage
			return;
		}

		if(!isTemporaryInvulnerable) {

			life -= damages;

			//set invulnerable to avoid taking all life in few milliseconds
			StartCoroutine(setTemporaryInvulnerable());

			if(life <= 0) {
				die();
			}
		}
		
	}
	
	IEnumerator setTemporaryInvulnerable() {
		
		isTemporaryInvulnerable = true;

		yield return new WaitForSeconds(0.5f);

		isTemporaryInvulnerable = false;
	}
	
	protected virtual void respawn() {
		
		//remove all colliders
		foreach(Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = true;
		}

	}

	protected virtual void die() {

		setAsDead();
	}
	
	protected virtual void setAsDead() {

		life = 0;

		//remove all colliders
		foreach(Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = false;
		}
	}


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
		
		if(InputsManager.Instance.isPaused) {
			return;
		}
		
		Vector2 newVector = getNewMoveVector();
		
		//update infos
		float dx = newVector.x;
		float dy = newVector.y;
		
		bool wasMoving = isMoving;
		
		isMoving = (dx != 0 || dy != 0);
		
		if(isMoving) {
			
			setAngleDegrees(Constants.vectorToAngle(newVector.x, newVector.y));
			
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
	
	
	protected abstract Vector2 getNewMoveVector();
	
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

			//the actions don't show when the player is blocked by animations
			PlayerActionsManager.Instance.setEnabled(this, false);
		}
		
		//call action
		if(characterAction.delegateAction != null) {
			characterAction.delegateAction(characterAction);
		}
		
		//render
		characterRenderer.animate(currentState, characterAction);
		
		yield return new WaitForSeconds(characterAction.durationSec);
		
		//call onfinish action
		if(characterAction.delegateOnFinish != null) {
			characterAction.delegateOnFinish(characterAction);
		}
		
		isControlsEnabledWhileAnimating = true;

		//enable again
		PlayerActionsManager.Instance.setEnabled(this, true);
		
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

