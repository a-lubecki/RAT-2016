using UnityEngine;
using System;
using System.Collections;
using Node;

public class PlayerBehavior : CharacterBehavior {
	
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
	
	private bool isRegainingStamina = false;
	private Coroutine coroutineRegainingStamina;


	public Player player {
		get {
			return character as Player;
		}
	}
	public PlayerRendererBehavior playerRendererBehavior {
		get {
			return playerRendererBehavior as PlayerRendererBehavior;
		}
	}

	public void init(Player player, PlayerRendererBehavior playerRendererBehavior, bool setRealPosition, int posX, int posY) {

		base.init(player, playerRendererBehavior, setRealPosition, posX, posY);

	}

	public override void onBehaviorAttached() {

		base.onBehaviorAttached();

		GameHelper.Instance.getXpDisplayManager().setTotalXp(player.xp);

		if(player.isDead()) {
			//respawn if the player was loaded and directly dead 
			die();
			return;
		}

		InvokeRepeating("manageStamina", PlayerBehavior.STAMINA_UPDATE_FREQUENCY_SEC, PlayerBehavior.STAMINA_UPDATE_FREQUENCY_SEC);

		//if coming from a save when the player has not full stamina, regain it
		startRegainingStaminaAfterDelay(1f);
	}

	public void earnXp(int newXp) {

		int lastXp = player.xp;

		player.earnXp(newXp);

		GameHelper.Instance.getXpDisplayManager().earnXp(lastXp, player.xp - lastXp);

		CancelInvoke("manageStamina");

		stopRegainingStamina();
		stopRunning();
	}

	protected override void die() {

		base.die();

		//TODO set xp on body then save
		player.xp = 0;
		GameHelper.Instance.getXpDisplayManager().setTotalXp(0);
		
		StartCoroutine(processRespawn());
	}
	
	protected override void setAsDead() {
		base.setAsDead();

		player.stamina = 0;
	}

	IEnumerator processRespawn() {

		LevelManager levelManager = GameHelper.Instance.getLevelManager();
		levelManager.preparePlayerToRespawn();
		
		disableControls();
		
		MessageDisplayer.Instance.displayBigMessage(Constants.tr("BigMessage.PlayerDead"), false);

		yield return new WaitForSeconds(2f);

		levelManager.processPlayerRespawn();
	}

	
	protected override Vector2 getNewMoveVector() {
		
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
		
		if(player.isRunning) {
			
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
		
		if(player.stamina > 0) {
			updateState(PlayerState.DASH);
		}
		
	}
	
	public void tryLeftAttack() {
		
		if(player.stamina > 0) {
			updateState(PlayerState.SHORT_ATTACK);
		}
		
	}
	
	public void tryRightAttack() {
		
		if(player.stamina > 0) {
			updateState(PlayerState.SHORT_ATTACK);
		}
		
	}
	
	protected override CharacterAction getCurrentCharacterAction() {
		
		if(player.currentState == BaseCharacterState.WALK) {
			return new CharacterAction(false, 0.4f);
		}
		
		if(player.currentState == BaseCharacterState.RUN) {
			return new CharacterAction(false, 0.2f);
		}
		
		if(player.currentState == PlayerState.DASH) {
			return new CharacterAction(true, 0.5f, delegate(CharacterAction action) {
				
				//remove stamina
				player.stamina -= PlayerBehavior.STAMINA_CONSUMPTION_DASH;
				
				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();
				
				float angle;
				if(player.isMoving) {
					angle = player.angleDegrees;
				} else {
					angle = Character.directionToAngle(currentDirection);
					
					//dash opposite
					angle += 180;
				}
				
				StartCoroutine(dashAfterDelay(angle, 50000, 0.1f));
				
			}, delegate(CharacterAction action) {
				
				startRegainingStaminaAfterDelay(PlayerBehavior.DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);
				
			});
		}
		
		if(player.currentState == PlayerState.SHORT_ATTACK) {
			return new CharacterAction(true, 0.9f, delegate(CharacterAction action) {
				
				//remove stamina
				player.stamina -= PlayerBehavior.STAMINA_CONSUMPTION_SHORT_ATTACK;
				
				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();
				
				float angle;				
				if(player.isMoving) {
					angle = player.angleDegrees;
				} else {
					angle = Character.directionToAngle(currentDirection);
				}
				StartCoroutine(dashAfterDelay(angle, 20000, 0.2f));
				
			}, delegate(CharacterAction action) {
				
				startRegainingStaminaAfterDelay(PlayerBehavior.DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);
				
			});
		}
		
		if(player.currentState == PlayerState.HEAVY_ATTACK) {
			return new CharacterAction(true, 1.5f, delegate(CharacterAction action) {
				
				//remove stamina
				player.stamina -= PlayerBehavior.STAMINA_CONSUMPTION_HEAVY_ATTACK;
				
				//after a dash, the player can't continue the same running
				stopRunning();
				stopRegainingStamina();
				
				float angle;				
				if(player.isMoving) {
					angle = player.angleDegrees;
				} else {
					angle = Character.directionToAngle(currentDirection);
				}
				StartCoroutine(dashAfterDelay(angle, 30000, 0.5f));
				
			}, delegate(CharacterAction action) {
				
				startRegainingStaminaAfterDelay(PlayerBehavior.DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC);
				
			});
		}
		
		if(player.currentState == PlayerState.DEFEND) {
			return new CharacterAction(false, 1);
		}
		
		//wait
		return new CharacterAction(false, 2);
		
	}
	
	protected override BaseCharacterState getNextState() {
		
		if(player.isMoving) { 
			if(player.currentState == BaseCharacterState.WALK) {
				return BaseCharacterState.WALK;
			}
			if(player.currentState == BaseCharacterState.RUN) {
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
		
		if(player.isDead()) {
			stopRegainingStamina();
			return;
		}
		
		if(player.isRunning) {
			
			player.stamina -= PlayerBehavior.STAMINA_CONSUMPTION_RUN;
			
			//can't run any more
			if(player.stamina <= 0) {
				stopRunning();
			}
			
		} else if(isRegainingStamina) {
			
			player.stamina += PlayerBehavior.STAMINA_REGAIN_REST;
			
			if(player.stamina >= player.maxStamina) {
				stopRegainingStamina();
			}
		}
		
	}
	
	
	void OnTriggerEnter2D(Collider2D other) {

		if(player == null) {
			return;
		}

		collide(other);
	}
	
	void OnTriggerStay2D(Collider2D other) {

		if(player == null) {
			return;
		}

		collide(other);
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_NPC.Equals(other.name)) {
			
			NpcBehavior npcBehavior = other.gameObject.GetComponent<NpcBehavior>();
			
			if(!npcBehavior.npc.isDead()) {
				//TODO TEST remove player life with (npc atk * npc level) - (player def + equipement def)
				takeDamages(10 * npcBehavior.npc.level);
			}
			
		}
	}


}

