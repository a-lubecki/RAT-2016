using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;

public class DefaultNpcRenderer : CharacterRenderer {
	
	private NpcBar npcBar;
	
	public void init(Npc npc, NpcBar npcBar) {

		if(npc == null) {
			throw new System.ArgumentException();
		}

		this.character = npc;
		this.npcBar = npcBar;
	}
	
	protected override void FixedUpdate() {

		base.FixedUpdate();

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

		npcBar.setValues(character.life, character.maxLife);
	
	}

	protected override CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState) {
		
		string textureName = "Enemy.Insect.Wait";
		
		//wait
		return new CharacterAnimation(
			textureName,
			new CharacterAnimationKey(0, 0));
	}

}
