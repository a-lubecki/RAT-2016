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
	public NpcBehavior npcBehavior {
		get {
			return characterBehavior as NpcBehavior;
		}
	}

	private NpcBar npcBar;

	public void init(Npc npc, NpcBehavior npcBehavior, NpcBar npcBar) {

		if(npcBar == null) {
			throw new System.ArgumentException();
		}

		this.npcBar = npcBar;

		base.init(npc, npcBehavior);

	}


	public override void onBehaviorAttached() {

		base.onBehaviorAttached();

		npcBar.setValues(npc.life, npc.maxLife, false);
	}

	protected override void FixedUpdate() {

		base.FixedUpdate();

		if(!isActiveAndEnabled) {
			return;
		}

		if(npc == null) {
			//not prepared
			return;
		}

		if(npcBar == null) {
			return;
		}
		if(!npcBar.enabled) {
			return;
		}

		//set the bar over the character
		Vector2 pos = transform.position;
		pos.y = (int) (pos.y + Constants.TILE_SIZE * 0.6f);
		npcBar.transform.position = pos;

		npcBar.setValues(npc.life, npc.maxLife, true);
	
	}

	protected override CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState) {
		
		string textureName = currentSpritePrefix + "Wait";
		
		//wait
		return new CharacterAnimation(
			textureName,
			new CharacterAnimationKey(0, 0));
	}

}
