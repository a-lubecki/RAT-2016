using UnityEngine;
using System.Collections;
using Level;

public class Npc : MonoBehaviour {
	
	private EntityRenderer entityRenderer;
	private NpcBar npcBar;

	public int life { get; private set; }
	public int maxLife { get; private set; }

	private bool isTemporaryInvulnerable = false;

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
	
	private void updateViews() {
		
		if(npcBar != null) {
			npcBar.setValues(life, maxLife);
		}
	}

	public void takeDamages(int damages) {
		
		if(damages == 0) {
			return;
		}
		
		
		if(!isTemporaryInvulnerable) {
			
			life -= damages;

			if(life < 0) {
				life = 0;
			} else if(life > maxLife) {
				life = maxLife;
			}

			updateViews();

			//set invulnerable to avoid taking all life in few milliseconds
			StartCoroutine(setTemporaryInvulnerable());
		}
		
	}
	
	IEnumerator setTemporaryInvulnerable() {
		
		isTemporaryInvulnerable = true;

		yield return new WaitForSeconds(0.5f);

		isTemporaryInvulnerable = false;
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {
			
			//TODO TEST remove enemy life
			takeDamages(1);

		}
		
	}

	void OnTriggerStay2D(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {
			
			//TODO TEST remove enemy life
			takeDamages(1);
			
		}
		
	}

}



