using System;
using Node;
using UnityEngine;

public class HubCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_HUB);
	}
		
	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_HUB;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_OBJECTS;
	}

	public GameObject createNewGameObject(NodeElementHub nodeElement, Hub hub) {

		if(nodeElement == null) {
			throw new System.ArgumentException();
		}
		if(hub == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y
			);
		
		HubBehavior hubBehavior = gameObject.GetComponent<HubBehavior>();
		hubBehavior.init(hub);
		
		return gameObject;
	}

}

