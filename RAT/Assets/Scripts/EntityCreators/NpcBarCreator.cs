using System;
using Level;
using UnityEngine;

public class NpcBarCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC_BAR);
	}

	protected override string getGameObjectName() {
		return Constants.PREFAB_NAME_NPC_BAR;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_HUB;
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

		return gameObject;
	}

}

