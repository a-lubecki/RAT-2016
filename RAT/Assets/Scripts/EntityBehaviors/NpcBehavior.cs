using UnityEngine;
using System;
using System.Collections;
using Node;

public class NpcBehavior : CharacterBehavior {

	public int level { get; private set; }

	public Npc npc {
		get {
			return character as Npc;
		}
	}
	public NpcRendererBehavior npcRendererBehavior {
		get {
			return characterRendererBehavior as NpcRendererBehavior;
		}
	}

	public void init(Npc npc, NpcRendererBehavior npcRendererBehavior, bool setRealPosition, int posX, int posY) {

		base.init(npc, npcRendererBehavior, setRealPosition, posX, posY);

	}

	public override void onBehaviorAttached() {

		base.onBehaviorAttached();

		reinitLife();

		if(npc.life <= 0) {
			setAsDead();
		} else {
			respawn();
		}

	}


	public void reinitLifeAndPosition() {

		updateRealPosition(
			npc.initialPosX * Constants.TILE_SIZE,
			- npc.initialPosY * Constants.TILE_SIZE,
			Character.directionToAngle(npc.initialDirection));

		reinitLife();
	}

	public void reinitLife() {
		
		npc.life = npc.maxLife;

		respawn();
	}

	protected override void die() {
		base.die();

		GameHelper.Instance.findPlayerBehavior().earnXp(500);//TODO test
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

		if(npc == null) {
			return;
		}

		collide(other);		
	}
	
	void OnTriggerStay2D(Collider2D other) {
		
		if(npc == null) {
			return;
		}

		collide(other);		
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER.Equals(other.name)) {
			
			Player player = GameHelper.Instance.getPlayer();
			
			if(!player.isDead()) {
				//TODO TEST remove player life
				gameObject.GetComponent<NpcBehavior>().takeDamages(100);
			}
			
		}
	}

}



