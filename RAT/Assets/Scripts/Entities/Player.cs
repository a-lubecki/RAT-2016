using UnityEngine;
using System.Collections;
using Level;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Player : Character {
	
	private static readonly int MAX_PLAYER_VALUE_FOR_BARS = 1500;
	
	public int skillPointHealth { get; protected set; }
	public int skillPointEnergy { get; protected set; }

	public int maxStamina { get; protected set; }
	public int stamina { get; protected set; }

	public string levelNameForlastHub;
	
	public void firstGameInit() {
		
		this.skillPointHealth = 5;
		this.skillPointEnergy = 5;
		
		computeStats();
		
		reinitLifeAndStamina();
	}

	public void init(int skillPointHealth, int skillPointEnergy, int life, int stamina) {
		
		this.skillPointHealth = skillPointHealth;
		this.skillPointEnergy = skillPointEnergy;
		
		computeStats();
		
		this.life = life;
		this.stamina = stamina;
		
		updateViews();
	}

	public void reinitLifeAndStamina() {
		
		this.life = maxLife;
		this.stamina = maxStamina;
		
		updateViews();
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
	
	protected override void die() {

		base.die();

		//set as an object
		GameHelper.Instance.getPlayerRenderer().gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.SORTING_LAYER_NAME_OBJECTS;

		MessageDisplayer.Instance.displayBigMessage("Vous êtes mort");

		//TODO disable keyboard / controller

		StartCoroutine(processRespawn());
	}

	IEnumerator processRespawn() {

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

