using System;
using Level;
using UnityEngine;

public class LinkCreator : BaseEntityCreator {

	
	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_LINK);
	}
		
	protected override string getGameObjectName() {
		return Constants.PREFAB_NAME_TILE_LINK;
	}
		
	protected override Sprite getDebugSprite() {
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_DEBUG + "Link.png", typeof(Sprite)) as Sprite;
	}
	
	public GameObject createNewGameObject(NodeElementLink nodeElement) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y
			);

		Link link = gameObject.GetComponent<Link>();
		link.nodeElementLink = nodeElement;

		return gameObject;
	}

}

