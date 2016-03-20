using System;
using Node;
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

		GameObject gameObject = createNewGameObject(0, 0);
		
		return gameObject;
	}

}

