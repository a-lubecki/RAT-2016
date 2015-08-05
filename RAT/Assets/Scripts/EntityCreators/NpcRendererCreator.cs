using System;
using Level;
using UnityEngine;

public class NpcRendererCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC_RENDERER);
	}

	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_NPC_RENDERER;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_CHARACTERS;
	}

	public GameObject createNewGameObject(NodeElementNpc nodeElement) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		int x = nodeElement.nodePosition.x;
		int y = nodeElement.nodePosition.y;

		GameObject gameObject = createNewGameObject(
			x, 
			y);

		EntityRenderer npcRenderer = gameObject.GetComponent<EntityRenderer>();

		npcRenderer.currentSpritePrefix = "Enemy.Insect";//TODO test
		//npcRenderer.currentSpritePrefix = nodeElement.;//TODO

		return gameObject;
	}

}

