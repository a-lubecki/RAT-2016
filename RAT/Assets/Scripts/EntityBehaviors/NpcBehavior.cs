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

	public void init(Npc npc, NpcRendererBehavior npcRendererBehavior) {

		base.init(npc, npcRendererBehavior);

	}

	void OnTriggerEnter2D(Collider2D collider) {
		
		collide(collider);		
	}
	
	void OnTriggerStay2D(Collider2D collider) {
		
		collide(collider);		
	}
	
	private void collide(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		Player player = GameHelper.Instance.getPlayer();
		
		if(!player.isDead()) {
			//TODO TEST remove player life
			npc.takeDamages(100);
		}

	}

}



