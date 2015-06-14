using System;

public class Constants {
	
	public static readonly int TILE_SIZE = 32;
	public static readonly float PIXEL_SIZE = 1;
	
	public static readonly string PATH_ASSETS = "Assets/";

	public static readonly string PATH_PREFABS = PATH_ASSETS + "Prefabs/";

	public static readonly string PATH_RES = PATH_ASSETS + "Res/";
	public static readonly string PATH_RES_TEST = PATH_RES + "Test/";
	public static readonly string PATH_RES_MAPS = PATH_RES + "Maps/";
	public static readonly string PATH_RES_ENVIRONMENTS = PATH_RES + "Environments/";
	public static readonly string PATH_RES_DEBUG = PATH_RES + "Debug/";
		
	
	private static readonly string PREFAB_EXTENSION = ".prefab";

	public static readonly string PREFAB_NAME_TILE_GROUND = "PrefabTileGround" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_WALL = "PrefabTileWall" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_LINK = "PrefabTileLink" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_DOOR = "PrefabTileDoor" + PREFAB_EXTENSION;

}

