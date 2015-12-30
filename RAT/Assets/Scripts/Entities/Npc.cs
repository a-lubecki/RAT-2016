using UnityEngine;
using System.Collections;
using Level;

public class Npc : Character {

	public NodeElementNpc nodeElementNpc { get; private set; }

	public void init(NodeElementNpc nodeElementNpc, CharacterRenderer characterRenderer) {

		if(nodeElementNpc == null) {
			throw new System.ArgumentException();
		}
		if(characterRenderer == null) {
			throw new System.ArgumentException();
		}

		this.nodeElementNpc = nodeElementNpc;
		this.characterRenderer = characterRenderer;

		reinitLife();
	}
	
	public void init(int life) {
		
		this.life = life;

		if(life <= 0) {
			setAsDead();
		} else {
			respawn();
		}

	}
	
	public void reinitLifeAndPosition() {

		setInitialPosition(
			nodeElementNpc.nodePosition.x * Constants.TILE_SIZE,
			- nodeElementNpc.nodePosition.y * Constants.TILE_SIZE,
			0);//TODO angle from nodeElementNpc

		reinitLife();
	}

	public void reinitLife() {
		
		this.maxLife = 100;//TODO set with nodeElementNpc
		this.life = 25;//TODO this.life = maxLife;

		respawn();
	}

	protected override void die() {
		base.die();

		GameHelper.Instance.getPlayer().earnXp(500);//TODO test
	}


	protected override Vector2 getNewMoveVector() {
		
		return new Vector2(0, 0);//TODO
	}
	
	protected override bool canRun() {
		return true;
	}
	
	protected override CharacterAction getCurrentCharacterAction() {
		return new CharacterAction(false, 100);
	}
	
	protected override BaseCharacterState getNextState() {
		return BaseCharacterState.WAIT;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		
		collide(other);		
	}
	
	void OnTriggerStay2D(Collider2D other) {
		
		collide(other);		
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER.Equals(other.name)) {
			
			Player player = GameHelper.Instance.getPlayer();
			
			if(!player.isDead()) {
				//TODO TEST remove player life
				gameObject.GetComponent<Npc>().takeDamages(100);
			}
			
		}
	}

}



