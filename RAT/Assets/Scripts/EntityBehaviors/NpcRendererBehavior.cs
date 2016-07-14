using UnityEngine;
using System;
using System.Collections;
using System.Collections.ObjectModel;

public class NpcRendererBehavior : CharacterRendererBehavior {

	public Npc npc {
		get {
			return character as Npc;
		}
	}

	private NpcBar npcBar;

	public void init(Npc npc, NpcBar npcBar) {

		if(npcBar == null) {
			throw new System.ArgumentException();
		}

		this.npcBar = npcBar;

		base.init(npc);

	}

	protected override void updateBehavior() {

		base.updateBehavior();

		if(npcBar.enabled) {

			bool mustReveal = false;//TODO

			//set the bar over the character
			Vector2 pos = transform.position;
			pos.y = (int) (pos.y + Constants.TILE_SIZE * 0.6f);
			npcBar.transform.position = pos;

			npcBar.setValues(npc.life, npc.maxLife, mustReveal);
		}

	}

	protected override CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState) {
		
		string textureName = currentSpritePrefix + "Wait";
		
		//wait
		return new CharacterAnimation(
			textureName,
			new CharacterAnimationKey(0, 0));
	}

}
