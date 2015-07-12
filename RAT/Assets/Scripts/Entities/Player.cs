using UnityEngine;
using System.Collections;
using Level;

public class Player : Character {

	private static readonly int MAX_PLAYER_HEALTH_FOR_BAR = 1500;

	public override void init() {

		this.life = 30;
		this.maxLife = 100;

		updateViews();
	}
	
	protected override void updateViews() {

		HUDBar hudBar = GameHelper.Instance.getHUDHealthBar().GetComponent<HUDBar>();

		if(hudBar != null) {
			hudBar.setBarSize(maxLife / (float) MAX_PLAYER_HEALTH_FOR_BAR);
			hudBar.setValues(life, maxLife);
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

