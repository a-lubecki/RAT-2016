using System;
using System.Collections.Generic;
using UnityEngine;
using MovementEffects;

public abstract class Character : BaseIdentifiableModel {

	public int initialMapPosX { get ; private set; }
	public int initialMapPosY { get ; private set; }
	public CharacterDirection initialDirection { get ; private set; }

	public int realPosX { get ; private set; }
	public int realPosY { get ; private set; }
	public int angleDegrees { get ; private set; }

	public bool isMoving { get; private set; }
	public bool isRunning { get; private set; }
	private bool mustStopTriggeredRunning = false;

	public BaseCharacterState currentState { get; private set; }

	public bool isInvulnerable { get; private set; }

	public int maxLife { get; private set; }
	public int life { get; private set; }

	public bool isControlsEnabled { get; private set; }
	public bool isControlsEnabledWhileAnimating { get; private set; }

	private IEnumerator<float> coroutineStateAnimation;

	public float animationPercentage { get; private set; }


	public Character(string id, List<Listener> listeners, int maxLife, int life, 
		int initialMapPosX, int initialMapPosY, CharacterDirection initialDirection, int realPosX, int realPosY, int angleDegrees) 
		: base(id, listeners) {

		this.initialMapPosX = initialMapPosX;
		this.initialMapPosY = initialMapPosY;
		this.initialDirection = initialDirection;

		this.realPosX = realPosX;
		this.realPosY = realPosY;
		this.angleDegrees = getAlignedAngleDegrees(angleDegrees);

		isMoving = false;
		isRunning = false;

		isInvulnerable = false;

		this.maxLife = maxLife;
		this.life = life;

		isControlsEnabled = true;
		isControlsEnabledWhileAnimating = true;

		changeState(BaseCharacterState.WAIT);

		animationPercentage = 0;
	}

	private static int getAlignedAngleDegrees(int angle) {

		int res = angle % 360;

		if(res < 0) {
			res += 360;
		}

		return res;
	}

	public static int directionToAngle(CharacterDirection direction) {

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


	public void updateRealPositionAngle(bool wasMoving, Vector2 newPos, int newAngle) {

		realPosX = newPos.x;
		realPosY = newPos.y;

		angleDegrees = getAlignedAngleDegrees(newAngle);

		//trigger walk/run state
		if(!wasMoving && currentState != BaseCharacterState.WALK) {
			changeState(BaseCharacterState.WALK);
		} else if(isRunning && currentState != BaseCharacterState.RUN) {
			changeState(BaseCharacterState.RUN);
		}

		updateBehaviors();

	}


	public void setMaxLife(int maxLife) { 

		if(maxLife <= 0) {
			this.maxLife = 0;
		} else {
			this.maxLife = maxLife;
		}

		updateBehaviors();
	}

	public void setLife(int life) { 

		if(life <= 0) {
			this.life = 0;
		} else if(life > maxLife) {
			this.life = maxLife;
		} else {
			this.life = life;
		}

		updateBehaviors();
	}

	public void reinitLife() {

		life = maxLife;

	}

	public bool isDead() {
		return (life <= 0);
	}

	public void setAsDead() {

		life = 0;

		updateBehaviors();
	}


	public void heal(int heal) {

		if(heal < 0) {
			throw new ArgumentException("Can't take negative heal");
		}
		if(heal == 0) {
			//not a heal or a damage
			return;
		}

		if(isDead()) {
			//already dead
			return;
		}

		life += heal;

		updateBehaviors();
	}

	public void takeDamages(int damages) {

		if(isDead()) {
			//already dead
			return;
		}

		if(damages < 0) {
			throw new ArgumentException("Can't take negative damages");
		}
		if(heal == 0) {
			//not a heal or a damage
			return;
		}

		if(isInvulnerable) {
			return;
		}
		if(damages == 0) {
			//not a heal or a damage
			return;
		}

		int oldLife = life;

		life -= damages;

		if(life < oldLife) {
			
			//set invulnerable to avoid taking all life in few milliseconds
			isInvulnerable = true;

			Timing.CallDelayed(1f, 
				delegate {
					isInvulnerable = false;
				}
			);
		}

		updateBehaviors();

		if(isDead()) {
			onDie();
		}

	}


	protected virtual void onDie() {

		//override
	}


	protected void startRunningAfterDelay(float delay) {
		
		if(isRunning) {
			return;
		}

		mustStopTriggeredRunning = false;

		Timing.CallDelayed(delay, 
			delegate {

				if(mustStopTriggeredRunning) {
					return;
				}

				if(isRunning) {
					return;
				}

				if(!canRun()) {
					return;
				}

				isRunning = true;

				didStartRunning();
			}
		);
	}

	protected void stopRunning() {

		mustStopTriggeredRunning = true;

		isRunning = false;

		didStopRunning();
	}

	protected abstract bool canRun();

	protected virtual void didStartRunning() {
		//override this
	}

	protected virtual void didStopRunning() {
		//override this
	}


	public abstract Vector2 getNewMoveVector();

	protected void changeState(BaseCharacterState state) {

		if(state == BaseCharacterState.UNDEFINED) {
			return;
		}

		if(state == currentState) {
			return;
		}

		currentState = state;

		updateBehaviors();


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

		//start once
		if (coroutineStateAnimation == null) {
			coroutineStateAnimation = Timing.RunCoroutine(animateCharacter(), Segment.FixedUpdate);
		}

	}


	private IEnumerator<float> animateCharacter() {

		BaseCharacterState lastState = BaseCharacterState.UNDEFINED;
		CharacterAction lastCharacterAction = null;
		float nextPercentage = 0;

		do {

			//check if changed between 2 loops
			if (currentState != lastState) {
				
				animationPercentage = 0;

				lastCharacterAction = getCurrentCharacterAction();
				nextPercentage = Constants.COROUTINE_PERIOD_S / lastCharacterAction.durationSec;

				lastState = currentState;
			}

			animationPercentage += nextPercentage;

			if (animationPercentage > 1) {
				animationPercentage = 1;
			}

			updateBehaviors();

			yield return Timing.WaitForSeconds(Constants.COROUTINE_PERIOD_S);

			if (animationPercentage >= 1) {

				//call onfinish action
				if(lastCharacterAction.delegateOnFinish != null) {
					lastCharacterAction.delegateOnFinish(lastCharacterAction);
				}

				isControlsEnabledWhileAnimating = true;

				//enable again
				PlayerActionsManager.Instance.setEnabled(this, true);

				changeState(BaseCharacterState.WAIT);
			}

		} while(currentState != BaseCharacterState.DEATH);


		//animate death

		animationPercentage = 0;
		CharacterAction characterActionDeath = getCurrentCharacterAction();
		nextPercentage = Constants.COROUTINE_PERIOD_S / characterActionDeath.durationSec;

		while (animationPercentage < 1) {

			if (animationPercentage > 1) {
				animationPercentage = 1;
			}

			updateBehaviors();

			yield return Timing.WaitForSeconds(Constants.COROUTINE_PERIOD_S);
		}

		//call onfinish action
		if(characterActionDeath.delegateOnFinish != null) {
			characterActionDeath.delegateOnFinish(characterActionDeath);
		}

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

