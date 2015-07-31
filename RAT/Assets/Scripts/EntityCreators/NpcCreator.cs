using System;
using Level;
using UnityEngine;

public class NpcCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC_COLLIDER);
	}

	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_NPC_COLLIDER;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_OBJECTS;
	}

	public GameObject createNewGameObject(NodeElementNpc nodeElement) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		int x = nodeElement.nodePosition.x;
		int y = nodeElement.nodePosition.y;

		GameObject gameObjectCollider = createNewGameObject(
			x, 
			y);

		GameObject gameObjectRenderer = new NpcRendererCreator().createNewGameObject(nodeElement);

		EntityCollider npcCollider = gameObjectCollider.GetComponent<EntityCollider>();
		EntityRenderer npcRenderer = gameObjectRenderer.GetComponent<EntityRenderer>();
		npcRenderer.entityCollider = npcCollider;
		npcCollider.entityRenderer = npcRenderer;

		GameObject gameObjectNpcBar = new NpcBarCreator().createNewGameObject(nodeElement);
		NpcBar npcBar = gameObjectNpcBar.GetComponent<NpcBar>();

		Npc npc = gameObjectCollider.GetComponent<Npc>();
		npc.init(nodeElement, npcRenderer, npcBar);

		return gameObjectCollider;
	}

}

