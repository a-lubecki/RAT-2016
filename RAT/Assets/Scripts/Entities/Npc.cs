using UnityEngine;
using System.Collections;
using Level;

public class Npc : Character {

	public NodeElementNpc nodeElementNpc { get; private set; }
	private EntityRenderer entityRenderer;
	private NpcBar npcBar;

	public AIControls getAIControls() {
		return (AIControls) entityRenderer.entityCollider;
	}

	public void init(NodeElementNpc nodeElementNpc, EntityRenderer entityRenderer, NpcBar npcBar) {

		if(nodeElementNpc == null) {
			throw new System.ArgumentException();
		}
		if(entityRenderer == null) {
			throw new System.ArgumentException();
		}

		this.nodeElementNpc = nodeElementNpc;
		this.entityRenderer = entityRenderer;
		this.npcBar = npcBar;

		reinitLife();
	}
	
	public void init(int life) {
		
		this.life = life;

		if(life <= 0) {
			setAsDead();
		} else {
			respawn();
		}

		updateViews();
	}
	
	public void reinitLifeAndPosition() {
		
		entityRenderer.entityCollider.setInitialPosition(
			nodeElementNpc.nodePosition.x * Constants.TILE_SIZE,
			- nodeElementNpc.nodePosition.y * Constants.TILE_SIZE,
			0);//TODO angle from nodeElementNpc

		reinitLife();
	}

	public void reinitLife() {
		
		this.maxLife = 100;//TODO set with nodeElementNpc
		this.life = 25;//TODO this.life = maxLife;

		respawn();

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
	
	protected override void respawn() {
		
		base.respawn();
		
		//set as a character
		entityRenderer.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.SORTING_LAYER_NAME_CHARACTERS;
		
	}

	protected override void die() {
		base.die();

		GameHelper.Instance.getPlayer().earnXp(500);//TODO test
	}
	
	protected override void setAsDead() {
		base.setAsDead();

		//set as an object
		entityRenderer.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.SORTING_LAYER_NAME_OBJECTS;

	}

	void OnTriggerEnter2D(Collider2D other) {
		
		collide(other);		
	}

	void OnTriggerStay2D(Collider2D other) {
		
		collide(other);		
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {
			
			Player player = GameHelper.Instance.getPlayer();
			
			if(!player.isDead()) {
				//TODO TEST remove player life
				takeDamages(10);
			}
			
		}
	}

}



