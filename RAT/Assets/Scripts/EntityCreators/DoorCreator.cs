using System;
using Level;
using UnityEngine;

public class DoorCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_DOOR);
	}
		
	protected override string getGameObjectName() {
		return Constants.PREFAB_NAME_TILE_DOOR;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_WALLS;
	}

	public GameObject createNewGameObject(NodeElementDoor nodeElement) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y,
			Quaternion.identity, 
			null,
			0
			);
		
		Door door = gameObject.GetComponent<Door>();
		door.setNodeElementDoor(nodeElement);

		if(nodeElement.nodeDoorStatus.value == NodeDoorStatus.DoorStatus.OPENED) {
			door.open(false);
		} else {
			door.close(false);
		}
		
		return gameObject;
	}

}

