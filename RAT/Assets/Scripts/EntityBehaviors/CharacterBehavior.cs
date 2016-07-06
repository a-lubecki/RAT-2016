using UnityEngine;
using System;
using System.Collections;

public abstract class CharacterBehavior : BaseEntityBehavior {

	public Character character {
		get {
			return (Character) entity;
		}
	}

	public CharacterRendererBehavior characterRendererBehavior { get; private set; }

	public CharacterDirection currentDirection { get; private set; }

	public bool isControlsEnabled { get; private set; }
	public bool isControlsEnabledWhileAnimating { get; private set; }

	private Coroutine coroutineRun;
	private Coroutine coroutineStateAnimation;

	private bool initSetRealPosition;
	private int initPosX;
	private int initPosY;

	protected void init(Character character, CharacterRendererBehavior characterRendererBehavior, bool setRealPosition, int posX, int posY) {

		if(characterRendererBehavior == null) {
			throw new ArgumentException();
		}

		this.characterRendererBehavior = characterRendererBehavior;

		isControlsEnabled = true;
		isControlsEnabledWhileAnimating = true;

		initSetRealPosition = setRealPosition;
		initPosX = posX;
		initPosY = posY;

		base.init(character);

	}

	public override void onBehaviorAttached() {

		base.onBehaviorAttached();

		if(initSetRealPosition) {
			updateRealPosition(initPosX, initPosY, character.angleDegrees);
		} else {
			updateMapPosition(character.initialPosX, character.initialPosY);
			updateDirection(character.initialDirection);
		}

		changeState(BaseCharacterState.WAIT);

	}


	public void takeDamages(int damages) {
		
		if(character.isDead()) {
			//already dead
			return;
		}

		int oldLife = character.life;

		character.takeDamages(damages);

		if(character.life < oldLife) {
			//set invulnerable to avoid taking all life in few milliseconds
			StartCoroutine(setTemporaryInvulnerable());
		}

		if(character.isDead()) {
			die();
		}
	}

	public void heal(int heal) {

		character.heal(heal);
	}

	IEnumerator setTemporaryInvulnerable() {
		
		character.isInvulnerable = true;

		yield return new WaitForSeconds(1f);//TODO change duration with the damages number

		character.isInvulnerable = false;
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

		character.setAsDead();

		//remove all colliders
		foreach(Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = false;
		}
	}


	public void updateRealPosition(int x, int y, int angleDegrees) {
		
		transform.position = new Vector2(x, y);
		
		character.angleDegrees = angleDegrees;
		
		updateStateWithDirection();
	}
	
	public void updateMapPosition(int xGeneration, int yGeneration) {
		
		transform.position = GameHelper.Instance.newPositionOnMap(xGeneration, yGeneration);
	}
	
	public void updateDirection(CharacterDirection direction) {
		
		if(direction == CharacterDirection.UP) {
			character.angleDegrees = 0;
		} else if(direction == CharacterDirection.RIGHT) {
			character.angleDegrees = 90;
		} else if(direction == CharacterDirection.DOWN) {
			character.angleDegrees = 180;
		} else if(direction == CharacterDirection.LEFT) {
			character.angleDegrees = 270;
		}
		
		updateStateWithDirection();
	}

	
	protected override void FixedUpdate() {

		base.FixedUpdate();

		if(character == null) {
			//not prepared
			return;
		}

		if(InputsManager.Instance.isPaused) {
			return;
		}
		
		Vector2 newVector = getNewMoveVector();
		
		//update infos
		float dx = newVector.x;
		float dy = newVector.y;
		
		bool wasMoving = character.isMoving;
		
		character.isMoving = (dx != 0 || dy != 0);
		
		if(character.isMoving) {
			
			character.angleDegrees = Constants.vectorToAngle(newVector.x, newVector.y);

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
			if(!wasMoving && character.currentState != BaseCharacterState.WALK) {
				changeState(BaseCharacterState.WALK);
			} else if(character.isRunning && character.currentState != BaseCharacterState.RUN) {
				changeState(BaseCharacterState.RUN);
			}
			
		}
		
	}
	
	
	protected void startRunningAfterDelay(float delay) {
		
		if(character.isRunning) {
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
		
		character.isRunning = false;
		
		didStopRunning();
	}
	
	private IEnumerator runAfterDelay(float delay) {
		
		if(delay > 0) {
			yield return new WaitForSeconds(delay);
		}
		
		if(!canRun()) {
			yield break;
		}
		
		character.isRunning = true;
		
		didStartRunning();
	}
	
	protected abstract bool canRun();
	
	protected virtual void didStartRunning() {
		//override this
	}
	
	protected virtual void didStopRunning() {
		//override this
	}
	

	protected abstract Vector2 getNewMoveVector();
	
	protected void changeState(BaseCharacterState state) {
		
		if(state == BaseCharacterState.UNDEFINED) {
			return;
		}

		if(state == character.currentState) {
			return;
		}
		
		updateState(state);
	}
	
	protected void updateState(BaseCharacterState state) {
		
		if(coroutineStateAnimation != null) {
			StopCoroutine(coroutineStateAnimation);
		}
		
		character.changeState(state);
		
		if(state != BaseCharacterState.UNDEFINED) {
			coroutineStateAnimation = StartCoroutine(animateCharacter());
		}
		
	}

	public void updateStateWithDirection() {

		CharacterDirection tempDirection = character.getCharacterDirection(currentDirection, 55);
		if(currentDirection != tempDirection) {

			currentDirection = tempDirection;

			//update state
			updateState(character.currentState);
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
		characterRendererBehavior.animate(character.currentState, characterAction);
		
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

