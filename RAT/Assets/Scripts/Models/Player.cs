using System;
using System.Collections.Generic;
using UnityEngine;
using MovementEffects;

public class Player : Character {

	public static readonly float STAMINA_UPDATE_FREQUENCY_SEC = 0.2f;
	public static readonly float DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC = 0.5f;
	public static readonly int STAMINA_REGAIN_REST = 12;
	public static readonly int STAMINA_CONSUMPTION_RUN = 6;
	public static readonly int STAMINA_CONSUMPTION_DASH = 30;
	public static readonly int STAMINA_CONSUMPTION_SHORT_ATTACK = 30;
	public static readonly int STAMINA_CONSUMPTION_HEAVY_ATTACK = 60;

	public static readonly float BASE_MOVE_SPEED = 100f;


	private float moveSpeed = BASE_MOVE_SPEED;

	private bool isPressingAnyDirection = false;


	public int skillPointsHealth { get; private set; }
	public int skillPointsEnergy { get; private set; }

	public int maxStamina { get; private set; }
	public int stamina { get; private set; } 

	public int xp { get; private set; } 
	private bool isRegainingStamina = false;
	private bool mustStopTriggeredRegainingStamina = false;


	public string levelNameForLastHub;


	public Player(int skillPointsHealth, int skillPointsEnergy, bool setLife, int life, bool setStamina, int stamina, int xp, float realPosX, float realPosY, int angleDegrees) 
		: base(Constants.GAME_OBJECT_NAME_PLAYER, null, 0, 0, 0, 0, CharacterDirection.UP, realPosX, realPosY, angleDegrees) {

		if(skillPointsHealth < 1) {
			this.skillPointsHealth = 1;
		} else {
			this.skillPointsHealth = skillPointsHealth;
		}

		if(skillPointsEnergy < 1) {
			this.skillPointsEnergy = 1;
		} else {
			this.skillPointsEnergy = skillPointsEnergy;
		}

		computePropertiesWithSkillPoints();

		if(setLife) {
			if (life < 0) {
				this.life = 0;
			} else {
				this.life = life;
			}
		} else {
			this.life = maxLife;
		}

		if(setStamina) {
			if (stamina < 0) {
				this.stamina = 0;
			} else {
				this.stamina = stamina;
			}
		} else {
			this.stamina = maxStamina;
		}

		if (xp < 0) {
			this.xp = 0;
		} else {
			this.xp = xp;
		}

		//if coming from a save when the player has not full stamina, regain it
		startRegainingStaminaAfterDelay(1f);

		Timing.RunCoroutine(manageStamina(), Segment.FixedUpdate);

	}


	/**
	 * Loop to update the stamina with the running / resting states
	 */
	protected IEnumerator<float> manageStamina() {

		while (!isDead()) {

			yield return Timing.WaitForSeconds(STAMINA_UPDATE_FREQUENCY_SEC);

			if(isRunning) {

				stamina -= STAMINA_CONSUMPTION_RUN;

				//can't run any more
				if(stamina <= 0) {
					stopRunning();
				}

			} else if(isRegainingStamina) {

				stamina += STAMINA_REGAIN_REST;

				if(stamina >= maxStamina) {
					stopRegainingStamina();
				}
			}

			updateBehaviors();

		}

		stopRegainingStamina();

	}


	public void reinitLifeAndStamina() {

		this.stamina = maxStamina;

		base.reinitLife();

	}

	private void computePropertiesWithSkillPoints() {

		//compute stats
		maxLife = 50 + 5 * skillPointsHealth + 2 * skillPointsEnergy;
		maxStamina = 80 + 5 * skillPointsEnergy;

		//security of poperties values
		if (life > maxLife) {
			life = maxLife;
		}
		if (stamina > maxStamina) {
			stamina = maxStamina;
		}
	}

	public void addSkillPointsHealth(int points) {

		if (points < 0) {
			throw new InvalidOperationException();
		}

		skillPointsHealth += points;

		computePropertiesWithSkillPoints();

		updateBehaviors();
	}

	public void addSkillPointsEnergy(int points) {

		if (points < 0) {
			throw new InvalidOperationException();
		}

		skillPointsEnergy += points;

		computePropertiesWithSkillPoints();

		updateBehaviors();
	}


	public void earnXp(int newXp) {

		if(newXp <= 0) {
			throw new ArgumentException();
		}

		int lastXp = xp;

		xp += newXp;

		GameHelper.Instance.getXpDisplayManager().earnXp(lastXp, xp - lastXp);

	}

	public override void setAsDead() {

		stamina = 0;

		base.setAsDead();
	}

	protected override void onDie() {

		//TODO set xp on body then save
		xp = 0;
		GameHelper.Instance.getXpDisplayManager().setTotalXp(0);

		LevelManager levelManager = GameHelper.Instance.getLevelManager();
		levelManager.preparePlayerToRespawn();

		disableControls();

		MessageDisplayer.Instance.displayBigMessage(Constants.tr("BigMessage.PlayerDead"), false);

		//respawn after a delay
		Timing.CallDelayed(2f, 
			delegate {
				levelManager.processPlayerRespawn();
			}
		);
	}


	public override Vector2 getNewMoveVector() {

		if(!isControlsEnabled || !isControlsEnabledWhileAnimating) {
			return new Vector2();
		}

		InputsManager inputsManager = GameHelper.Instance.getInputsManager();

		float angleDegrees = inputsManager.inputActionPlayerMove.angleDegrees;
		float analogicFactor = inputsManager.inputActionPlayerMove.analogicFactor;
		bool hasStartedRunning = false;

		isPressingAnyDirection = false;


		// analogic directions
		if(analogicFactor > 0) {

			isPressingAnyDirection = true;

			if(inputsManager.inputActionPlayerRun.isRunning) {
				startRunningAfterDelay(0.4f);
				hasStartedRunning = true;
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

		float x = analogicFactor * moveSpeed * Mathf.Cos(angleDegrees * Mathf.Deg2Rad);
		float y = analogicFactor * moveSpeed * Mathf.Sin(angleDegrees * Mathf.Deg2Rad);

		return new Vector2(x, y);

	}

	public void tryDash() {

		if (stamina > 0) {
			changeState(PlayerState.DASH);
		}

	}

	public void tryLeftAttack() {

		if (stamina > 0) {
			changeState(PlayerState.SHORT_ATTACK);
		}

	}

	public void tryRightAttack() {

		if (stamina > 0) {
			changeState(PlayerState.SHORT_ATTACK);
		}

	}

	protected override CharacterAction getCurrentCharacterAction() {

		if (currentState == BaseCharacterState.WALK) {
			return new CharacterAction(false, 0.4f);
		}

		if (currentState == BaseCharacterState.RUN) {
			return new CharacterAction(false, 0.2f);
		}

		if (currentState == PlayerState.DASH) {
			return new CharacterAction(true, 0.5f, delegate(CharacterAction action) {

				//remove stamina
				stamina -= STAMINA_CONSUMPTION_DASH;

				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();

				float angle;
				if(isMoving) {
					angle = angleDegrees;
				} else {
					//dash opposite
					angle = angleDegrees + 180;
				}

				//dash after delay
				Timing.CallDelayed(0.1f,
					delegate {
						dashAfterDelay(angle, 50000);
					}
				);

			}, delegate(CharacterAction action) {

				startRegainingStaminaAfterDelay(DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);

			});
		}

		if (currentState == PlayerState.SHORT_ATTACK) {
			return new CharacterAction(true, 0.9f, delegate(CharacterAction action) {

				//remove stamina
				stamina -= STAMINA_CONSUMPTION_SHORT_ATTACK;

				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();

				//dash after delay
				Timing.CallDelayed(0.2f,
					delegate {
						dashAfterDelay(angleDegrees, 20000);
					}
				);

			}, delegate(CharacterAction action) {

				startRegainingStaminaAfterDelay(DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);

			});
		}

		if(currentState == PlayerState.HEAVY_ATTACK) {
			return new CharacterAction(true, 1.5f, delegate(CharacterAction action) {

				//remove stamina
				stamina -= STAMINA_CONSUMPTION_HEAVY_ATTACK;

				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();

				//dash after delay
				Timing.CallDelayed(0.5f,
					delegate {
						dashAfterDelay(angleDegrees, 30000);
					}
				);

			}, delegate(CharacterAction action) {

				startRegainingStaminaAfterDelay(DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);

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


	protected override bool canMove() {
		return !InputsManager.Instance.isPaused;
	}


	protected override bool canRun() {

		if(!isPressingAnyDirection) {
			//no need to run if no direction pressed
			return false;
		}

		if(stamina <= 0) {
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

	protected void dashAfterDelay(float angle, int force) {

		CharacterBehavior characterBehavior = findBehavior<CharacterBehavior>();
		if (characterBehavior == null) {
			throw new InvalidOperationException();
		}

		Rigidbody2D rigidBody = characterBehavior.GetComponent<Rigidbody2D>();

		Vector2 newForce = Constants.angleToVector(angle, force);

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

		if(stamina >= maxStamina) {
			return;
		}


		mustStopTriggeredRegainingStamina = false;

		Timing.CallDelayed(1f, 
			delegate {

				if (mustStopTriggeredRegainingStamina) {
					return;
				}

				isRegainingStamina = true;
			}
		);

	}

	protected void stopRegainingStamina() {

		mustStopTriggeredRegainingStamina = true;

		isRegainingStamina = false;
	}


	public void onCollideWithNpc(Npc npc) {

		if(npc.isDead()) {
			return;
		}
			
		//TODO TEST remove player life with (npc atk * npc level) - (player def + equipement def)
		takeDamages(10 * npc.level);

	}

}

