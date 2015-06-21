using System;
using Level;
using TiledMap;
using UnityEngine;

public class WallCreator : BaseEntityCreator {

	
	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_WALL);
	}
		
	protected override string getGameObjectName() {
		return Constants.PREFAB_NAME_TILE_WALL;
	}

	public GameObject createNewGameObject(int x, int y, Tile tile) {

		if(tile == null) {
			throw new System.ArgumentException();
		}

		TileDescriptor tileDescriptor = tile.tileDescriptor;
		if(tileDescriptor == null) {
			throw new System.InvalidOperationException();
		}
		
		return createNewGameObject(
			x, 
			y
			);
	}

}

