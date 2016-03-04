using System;
using Node;
using UnityEngine;
using TiledMap;

public class GroundCreator : BaseEntityCreator {

	
	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_GROUND);
	}

	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_GROUND;
	}

	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_GROUND;
	}
	
	public GameObject createNewGameObject(int x, int y, Tile tile, int orderInLayer) {

		if(tile == null) {
			throw new System.ArgumentException();
		}

		TileDescriptor tileDescriptor = tile.tileDescriptor;
		if(tileDescriptor == null) {
			throw new System.InvalidOperationException();
		}
		
		return createNewGameObject(
			x, 
			y,
			tile.rotation,
			tileDescriptor.tileSprite,
			orderInLayer
			);
	}

}

