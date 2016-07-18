using UnityEngine;
using System;
using System.Collections;
using Node;

public class PlayerBehavior : CharacterBehavior {
	
	public Player player {
		get {
			return character as Player;
		}
	}

	public void init(Player player) {

		base.init(player);

	}

	protected override void updateBehavior() {
		
		base.updateBehavior();

		//TODO

	}


	void OnTriggerEnter2D(Collider2D collider) {

		collide(collider);
	}
	
	void OnTriggerStay2D(Collider2D collider) {

		collide(collider);
	}
	
	private void collide(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_NPC.Equals(collider.name)) {
			return;
		}

		Npc npc = collider.GetComponent<NpcBehavior>().npc;

		player.onCollideWithNpc(npc);

	}


}

