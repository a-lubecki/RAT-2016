using System;
using Node;
using UnityEngine;

public class LinkCreator : BaseEntityCreator {

	
	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_LINK);
	}
		
	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_LINK;
	}
		
	protected override Sprite getDebugSprite() {
		return GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_DEBUG + "Link");
	}
	
	public GameObject createNewGameObject(NodeElementLink nodeElement, Link link) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}
		if(link == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y
			);

		//change scale :
		Transform transform = gameObject.transform;

		Vector2 localScale = transform.localScale;
		Vector2 pos = transform.position;
		
		if(nodeElement.nodeWidth != null) {
			float scale = nodeElement.nodeWidth.value * Constants.TILE_SIZE;
			localScale.x = scale;
			pos.x += scale / 2f - 0.5f * Constants.TILE_SIZE; 
		}
		if(nodeElement.nodeHeight != null) {
			float scale = nodeElement.nodeHeight.value * Constants.TILE_SIZE;
			localScale.y = scale;
			pos.y += scale / 2f - 0.5f * Constants.TILE_SIZE;
		}
		
		transform.localScale = localScale;
		transform.localPosition = pos;


		LinkBehavior linkBehavior = gameObject.GetComponent<LinkBehavior>();
		linkBehavior.init(link);

		return gameObject;
	}

}

