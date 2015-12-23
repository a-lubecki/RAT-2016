using System;
using SmartLocalization;

public class Constants {
	
	public static readonly int TILE_SIZE = 32;
	public static readonly float PIXEL_SIZE = 1;
	
	public static readonly string PATH_ASSETS = "Assets/";

	public static readonly string PATH_PREFABS = PATH_ASSETS + "Prefabs/";

	public static readonly string PATH_RES = PATH_ASSETS + "Res/";
	public static readonly string PATH_RES_TEST = PATH_RES + "Test/";
	public static readonly string PATH_RES_MAPS = PATH_RES + "Maps/";
	public static readonly string PATH_RES_ENVIRONMENTS = PATH_RES + "Environments/";
	public static readonly string PATH_RES_CHARACTERS = PATH_RES + "Characters/";
	public static readonly string PATH_RES_SCRIPTS = PATH_RES + "Scripts/";
	public static readonly string PATH_RES_DEBUG = PATH_RES + "Debug/";
		
	
	private static readonly string PREFAB_EXTENSION = ".prefab";

	public static readonly string PREFAB_NAME_TILE_GROUND = "PrefabTileGround" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_WALL = "PrefabTileWall" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_HUB = "PrefabTileHub" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_LINK = "PrefabTileLink" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_TILE_DOOR = "PrefabTileDoor" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_NPC_COLLIDER = "NpcCollider" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_NPC_RENDERER = "NpcRenderer" + PREFAB_EXTENSION;
	public static readonly string PREFAB_NAME_NPC_BAR = "NpcBar" + PREFAB_EXTENSION;

	
	public static readonly string GAME_OBJECT_NAME_MAIN_CAMERA = "MainCamera";
	public static readonly string GAME_OBJECT_NAME_HUD = "CanvasHUD";
	public static readonly string GAME_OBJECT_NAME_HUD_HEALTH_BAR = "HUDBarHealth";
	public static readonly string GAME_OBJECT_NAME_HUD_STAMINA_BAR = "HUDBarStamina";
	public static readonly string GAME_OBJECT_NAME_PLAYER_COLLIDER = "PlayerCollider";
	public static readonly string GAME_OBJECT_NAME_PLAYER_RENDERER = "PlayerRenderer";
	public static readonly string GAME_OBJECT_NAME_LEVEL_MANAGER = "LevelManager";
	public static readonly string GAME_OBJECT_NAME_MAP = "Map";
	public static readonly string GAME_OBJECT_NAME_GROUND = "PrefabTileGround";
	public static readonly string GAME_OBJECT_NAME_WALL = "PrefabTileWall";
	public static readonly string GAME_OBJECT_NAME_HUB = "PrefabTileHub";
	public static readonly string GAME_OBJECT_NAME_LINK = "PrefabTileLink";
	public static readonly string GAME_OBJECT_NAME_DOOR = "PrefabTileDoor";
	public static readonly string GAME_OBJECT_NAME_NPC_COLLIDER = "NpcCollider";
	public static readonly string GAME_OBJECT_NAME_NPC_RENDERER = "NpcRenderer";
	public static readonly string GAME_OBJECT_NAME_NPC_BAR = "NpcBar";

	public static readonly string GAME_OBJECT_NAME_MESSAGE_DISPLAYER = "MessageDisplayer";
	public static readonly string GAME_OBJECT_NAME_TEXT_MESSAGE_BIG = "TextMessageBig";
	public static readonly string GAME_OBJECT_NAME_BACKGROUND_MESSAGE_BIG = "BackgroundMessageBig";
	public static readonly string GAME_OBJECT_NAME_TEXT_MESSAGE_NORMAL = "TextMessageNormal";
	public static readonly string GAME_OBJECT_NAME_BACKGROUND_MESSAGE_NORMAL = "BackgroundMessageNormal";
	public static readonly string GAME_OBJECT_NAME_TEXT_MESSAGE_ACTION = "TextMessageAction";
	public static readonly string GAME_OBJECT_NAME_BACKGROUND_MESSAGE_ACTION = "BackgroundMessageAction";
	public static readonly string GAME_OBJECT_NAME_TEXT_XP_TOTAL = "TextXpTotal";
	public static readonly string GAME_OBJECT_NAME_TEXT_XP_EARNED = "TextXpEarned";
	public static readonly string GAME_OBJECT_NAME_XP_DISPLAY_MANAGER = "XpDisplayManager";
	
	public static readonly string GAME_OBJECT_NAME_MENU = "Menu";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT = "SubMenuTitleLeft";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT = "SubMenuTitleRight";
	public static readonly string GAME_OBJECT_NAME_MENU_ARROW_LEFT = "MenuArrowLeft";
	public static readonly string GAME_OBJECT_NAME_MENU_ARROW_RIGHT = "MenuArrowRight";

	public static readonly string GAME_OBJECT_NAME_SUB_MENU_KEEPER = "SubMenuKeeper";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_CHARACTER_STATUS = "SubMenuTypeCharacterStatus";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_GAME_OPTIONS = "SubMenuTypeGameOptions";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT = "SubMenuTypeInventoryManagment";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_ITEMS_CHEST = "SubMenuTypeItemsChest";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_LEVEL_UPGRADE = "SubMenuTypeLevelUpgrade";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_MERCHANT = "SubMenuTypeMerchant";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_STORY_ITEMS_LIST = "SubMenuTypeStoryItemsList";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_TELEPORT = "SubMenuTypeTeleport";

	public static readonly string SORTING_LAYER_NAME_GROUND = "ground";
	public static readonly string SORTING_LAYER_NAME_WALLS = "walls";
	public static readonly string SORTING_LAYER_NAME_OBJECTS = "objects";
	public static readonly string SORTING_LAYER_NAME_CHARACTERS = "characters";
	public static readonly string SORTING_LAYER_NAME_HUB = "hub";

	public static readonly string FIRST_LEVEL_NAME = "Part1.Laboratory1";//the very first level

	
	public static string tr(string key) {
		return LanguageManager.Instance.GetTextValue(key);
	}
}

