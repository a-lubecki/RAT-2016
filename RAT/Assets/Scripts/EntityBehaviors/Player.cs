using UnityEngine;
using System.Collections;
using Node;

public class Player : Character {
	
	public static readonly float STAMINA_UPDATE_FREQUENCY_SEC = 0.2f;
	public static readonly float DELAY_STAMINA_RECOVERY_AFTER_ACTION_SEC = 0.5f;
	public static readonly int STAMINA_REGAIN_REST = 5;
	public static readonly int STAMINA_CONSUMPTION_RUN = 6;
	public static readonly int STAMINA_CONSUMPTION_DASH = 30;
	public static readonly int STAMINA_CONSUMPTION_SHORT_ATTACK = 30;
	public static readonly int STAMINA_CONSUMPTION_HEAVY_ATTACK = 60;

	public static readonly float BASE_MOVE_SPEED = 100f;

	private float moveSpeed = BASE_MOVE_SPEED;
	
	private bool isPressingAnyDirection = false;
	
	private bool isRegainingStamina = false;
	private Coroutine coroutineRegainingStamina;


	private int _skillPointHealth;
	public int skillPointHealth { 
		get {
			return _skillPointHealth;
		}
		set {
			if(value < 1) {
				_skillPointHealth = 1;
			} else {
				_skillPointHealth = value;
			}
		}
	}
	
	private int _skillPointEnergy;
	public int skillPointEnergy { 
		get {
			return _skillPointEnergy;
		}
		set {
			if(value < 1) {
				_skillPointEnergy = 1;
			} else {
				_skillPointEnergy = value;
			}
		}
	}

	private int _maxStamina;
	public int maxStamina { 
		get {
			return _maxStamina;
		}
		set {
			if(value <= 0) {
				_maxStamina = 0;
			} else {
				_maxStamina = value;
			}
		}
	}

	private int _stamina;
	public int stamina { 
		get {
			return _stamina;
		}
		set {
			if(value <= 0) {
				_stamina = 0;
			} else if(value > _maxStamina) {
				_stamina = _maxStamina;
			} else {
				_stamina = value;
			}
		}
	}
	
	private int _xp;
	public int xp { 
		get {
			return _xp;
		}
		set {
			if(value <= 0) {
				_xp = 0;
			} else {
				_xp = value;
			}
		}
	}

	public string levelNameForLastHub;

	public void initStats(int skillPointHealth, int skillPointEnergy) {
		
		this.skillPointHealth = skillPointHealth;
		this.skillPointEnergy = skillPointEnergy;

		computeStats();

		reinitLifeAndStamina();
	}
		
	public void init(int life, int stamina, int xp) {

		this.life = life;
		this.stamina = stamina;

		this.xp = xp;
		GameHelper.Instance.getXpDisplayManager().setTotalXp(xp);

		if(isDead()) {
			//respawn if the player was loaded and directly dead 
			die();
		}
	}

	public void reinitLifeAndStamina() {
		
		this.life = maxLife;
		this.stamina = maxStamina;
	}

	private void computeStats() {

		maxLife = 50 + 5 * skillPointHealth + 2 * skillPointEnergy;
		maxStamina = 80 + 5 * skillPointEnergy;
	}

	public void earnXp(int newXp) {
		int lastXp = xp;
		xp += newXp;
		GameHelper.Instance.getXpDisplayManager().earnXp(lastXp, xp - lastXp);
	}

	protected override void die() {

		base.die();

		//TODO set xp on body then save
		xp = 0;
		GameHelper.Instance.getXpDisplayManager().setTotalXp(0);
		
		StartCoroutine(processRespawn());
	}
	
	protected override void setAsDead() {
		base.setAsDead();

		stamina = 0;
	}

	IEnumerator processRespawn() {

		LevelManager levelManager = GameHelper.Instance.getLevelManager();
		levelManager.preparePlayerToRespawn();
		
		disableControls();
		
		MessageDisplayer.Instance.displayBigMessage(Constants.tr("BigMessage.PlayerDead"), false);

		yield return new WaitForSeconds(2f);

		levelManager.processPlayerRespawn();
	}


	public void setInitialPosition(NodePosition nodePosition, NodeDirection nodeDirection) {
		
		if(nodePosition != null) {
			setMapPosition(nodePosition.x, nodePosition.y);
		} else {
			setMapPosition(0, 0);
		}
		
		if(nodeDirection != null) {
			setDirection(nodeDirection.value);
		} else {
			setDirection(Direction.UP);
		}
	}
	
	protected override void Start() {
		base.Start();
		
		InvokeRepeating("manageStamina", Player.STAMINA_UPDATE_FREQUENCY_SEC, Player.STAMINA_UPDATE_FREQUENCY_SEC);
		
		//if coming from a save when the player has not full stamina, regain it
		startRegainingStaminaAfterDelay(1f);
	}
	
	
	protected void OnDisable() {
		
		stopRunning();
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
		
		Player player = GameHelper.Instance.getPlayer();
		if(player.stamina > 0) {
			updateState(PlayerState.DASH);
		}
		
	}
	
	public void tryLeftAttack() {
		
		Player player = GameHelper.Instance.getPlayer();
		
		if(player.stamina > 0) {
			updateState(PlayerState.SHORT_ATTACK);
		}
		
	}
	
	public void tryRightAttack() {
		
		Player player = GameHelper.Instance.getPlayer();
		
		if(player.stamina > 0) {
			updateState(PlayerState.SHORT_ATTACK);
		}
		
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
	
	
	void OnTriggerEnter2D(Collider2D other) {
		
		collide(other);
	}
	
	void OnTriggerStay2D(Collider2D other) {
		
		collide(other);
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_NPC.Equals(other.name)) {
			
			Npc npc = other.gameObject.GetComponent<Npc>();
			
			if(!npc.isDead()) {
				//TODO TEST remove player life
				Player player = GameHelper.Instance.getPlayer();
				player.takeDamages(10);
			}
			
		}
	}


}

