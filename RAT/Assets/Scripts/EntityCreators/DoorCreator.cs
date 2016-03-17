using System;
using Node;
using UnityEngine;

public class DoorCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_DOOR);
	}
		
	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_DOOR;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_WALLS;
	}

	public GameObject createNewGameObject(NodeElementDoor nodeElement, Door door) {

		if(nodeElement == null) {
			throw new System.ArgumentException();
		}
		if(door == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y,
			Quaternion.identity, 
			null,
			0
			);
		
		DoorBehavior doorBehavior = gameObject.GetComponent<DoorBehavior>();
		doorBehavior.init(door);

		return gameObject;
	}

}

