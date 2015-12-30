using System;
using Level;
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
		
		Npc npc = gameObjectCollider.GetComponent<Npc>();
		CharacterRenderer npcRenderer = gameObjectRenderer.GetComponent<CharacterRenderer>();
		npcRenderer.character = npc;
		npc.characterRenderer = npcRenderer;

		GameObject gameObjectNpcBar = new NpcBarCreator().createNewGameObject(nodeElement);
		NpcBar npcBar = gameObjectNpcBar.GetComponent<NpcBar>();

		npc.init(nodeElement, npcRenderer, npcBar);

		return gameObjectCollider;
	}

}

