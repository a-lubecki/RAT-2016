using System;
using Level;
using UnityEngine;

public class NpcRendererCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC_RENDERER);
	}

	protected override string getGameObjectName() {
		return Constants.PREFAB_NAME_NPC_RENDERER;
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

		GameObject gameObject = createNewGameObject(
			x, 
			y);
		
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		Texture2D texture = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_CHARACTERS + "Enemy.Insect.png");
		spriteRenderer.sprite = Sprite.Create(
			texture, 
			new Rect(0, 0, Constants.TILE_SIZE, Constants.TILE_SIZE),
			new Vector2(0.5f, 0.5f),
			Constants.TILE_SIZE);


		return gameObject;
	}

}

