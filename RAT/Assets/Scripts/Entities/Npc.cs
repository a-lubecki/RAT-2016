using UnityEngine;
using System.Collections;
using Level;

public class Npc : Character {
	
	private EntityRenderer entityRenderer;
	private NpcBar npcBar;

	public void init(EntityRenderer entityRenderer, NpcBar npcBar) {

		if(entityRenderer == null) {
			throw new System.ArgumentException();
		}

		this.entityRenderer = entityRenderer;
		this.npcBar = npcBar;

		this.life = 5;
		this.maxLife = 10;

		updateViews();
	}

	void Update() {

		//set the bar over the character
		if(npcBar != null && npcBar.enabled) {
			Vector2 pos = entityRenderer.transform.position;
			pos.y = (int) (pos.y + Constants.TILE_SIZE * 0.6f);
			npcBar.transform.position = pos;
		}
	}
	
	protected override void updateViews() {
		
		if(npcBar != null) {
			npcBar.setValues(life, maxLife);
		}
	}
	
	protected override void die() {
		
		base.die();
		
		//set as an object
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		collide(other);		
	}

	void OnTriggerStay2D(Collider2D other) {
		
		collide(other);		
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {
			
			Player player = other.gameObject.GetComponent<Player>();
			
			if(!player.isDead()) {
				//TODO TEST remove player life
				takeDamages(1);
			}
			
		}
	}

}



