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

	protected override Sprite getDebugSprite() {
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_DEBUG + "Door.png", typeof(Sprite)) as Sprite;
	}

	public GameObject createNewGameObject(NodeElementDoor nodeElement) {
		
		if(nodeElement == null) {
			throw new System.InvalidOperationException();
		}
		
		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y,
			Quaternion.identity, 
			null, //TODO
			0
			);
		
		Door door = gameObject.GetComponent<Door>();
		door.nodeElementDoor = nodeElement;
		
		return gameObject;
	}

}

