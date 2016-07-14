using System;
using Node;
using UnityEngine;

public class NpcCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC);
	}

	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_NPC;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_OBJECTS;
	}

	public GameObject createNewGameObject(NodeElementNpc nodeElement, Npc npc) {

		if(nodeElement == null) {
			throw new System.ArgumentException();
		}
		if(npc == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObjectCollider = createNewGameObject(0, 0);
		GameObject gameObjectRenderer = new NpcRendererCreator().createNewGameObject(nodeElement);
		
		NpcBehavior npcBehavior = gameObjectCollider.GetComponent<NpcBehavior>();
		NpcRendererBehavior npcRendererBehavior = gameObjectRenderer.GetComponent<NpcRendererBehavior>();

		GameObject gameObjectNpcBar = new NpcBarCreator().createNewGameObject(nodeElement);
		NpcBar npcBar = gameObjectNpcBar.GetComponent<NpcBar>();

		npcRendererBehavior.init(npc, npcBehavior, npcBar);
		npcBehavior.init(npc, npcRendererBehavior);

		return gameObjectCollider;
	}

}

