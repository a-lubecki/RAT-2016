using UnityEngine;
using System.Collections;
using Level;

public class Player : Character {
	
	public static readonly float STAMINA_UPDATE_FREQUENCY_S = 0.2f;
	public static readonly int STAMINA_REGAIN_REST = 5;
	public static readonly int STAMINA_CONSUMPTION_RUN = 6;
	public static readonly int STAMINA_CONSUMPTION_DASH = 30;

	private static readonly int MAX_PLAYER_VALUE_FOR_BARS = 1500;
	
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
			updateViews();
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
			updateViews();
		}
	}

	public string levelNameForLastHub;

	public void initStats(int skillPointHealth, int skillPointEnergy) {
		
		this.skillPointHealth = skillPointHealth;
		this.skillPointEnergy = skillPointEnergy;

		computeStats();

		reinitLifeAndStamina();
	}
		
	public void init(int life, int stamina) {

		this.life = life;
		this.stamina = stamina;
	}

	public void reinitLifeAndStamina() {
		
		this.life = maxLife;
		this.stamina = maxStamina;
	}

	private void computeStats() {

		maxLife = 50 + 5 * skillPointHealth + 2 * skillPointEnergy;
		maxStamina = 80 + 5 * skillPointEnergy;
	}
	
	protected override void updateViews() {

		//update health bar
		HUDBar healthBar = GameHelper.Instance.getHUDHealthBar().GetComponent<HUDBar>();
		
		if(healthBar != null) {
			healthBar.setBarSize(maxLife / (float) MAX_PLAYER_VALUE_FOR_BARS);
			healthBar.setValues(life, maxLife);
		}

		//update stamina bar
		HUDBar staminaBar = GameHelper.Instance.getHUDStaminaBar().GetComponent<HUDBar>();
		
		if(staminaBar != null) {
			staminaBar.setBarSize(maxStamina / (float) MAX_PLAYER_VALUE_FOR_BARS);
			staminaBar.setValues(stamina, maxStamina);
		}
	}

	protected override void respawn() {

		base.respawn();
		
		//set as a character
		GameHelper.Instance.getPlayerRenderer().gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.SORTING_LAYER_NAME_CHARACTERS;

	}

	protected override void die() {

		base.die();
		
		//set as an object
		GameHelper.Instance.getPlayerRenderer().gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.SORTING_LAYER_NAME_OBJECTS;

		MessageDisplayer.Instance.displayBigMessage(Constants.tr("BigMessage.PlayerDead"), false);

		StartCoroutine(processRespawn());
	}

	IEnumerator processRespawn() {
		
		GameHelper.Instance.getPlayerControls().disableControls();

		yield return new WaitForSeconds(2f);

		GameHelper.Instance.getLevelManager().processPlayerRespawn();
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		collide(other);
	}
	
	void OnTriggerStay2D(Collider2D other) {

		collide(other);
	}

	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_NPC_COLLIDER.Equals(other.name)) {
			
			Npc npc = other.gameObject.GetComponent<Npc>();
			
			if(!npc.isDead()) {
				//TODO TEST remove player life
				takeDamages(1);
			}
			
		}
	}


}

