using System;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using UnityEngine;


public class Constants {
	
	public static readonly int TILE_SIZE = 32;
	public static readonly float PIXEL_SIZE = 1;

	public static readonly string PATH_RES = "";
	public static readonly string PATH_RES_PREFABS = PATH_RES + "Prefabs/";
	public static readonly string PATH_RES_TEST = PATH_RES + "Test/";
	public static readonly string PATH_RES_MAPS = PATH_RES + "Maps/";
	public static readonly string PATH_RES_ITEMS = PATH_RES + "Items/";
	public static readonly string PATH_RES_ENVIRONMENTS = PATH_RES + "Environments/";
	public static readonly string PATH_RES_CHARACTERS = PATH_RES + "Characters/";
	public static readonly string PATH_RES_MENUS = PATH_RES + "Menus/";
	public static readonly string PATH_RES_SPLASHSCREEN = PATH_RES + "Splashscreen/";
	public static readonly string PATH_RES_SCRIPTS = PATH_RES + "Scripts/";
	public static readonly string PATH_RES_DEBUG = PATH_RES + "Debug/";
	

	public static readonly string PREFAB_NAME_TILE_GROUND = "PrefabTileGround";
	public static readonly string PREFAB_NAME_TILE_WALL = "PrefabTileWall";
	public static readonly string PREFAB_NAME_TILE_HUB = "PrefabTileHub";
	public static readonly string PREFAB_NAME_TILE_LINK = "PrefabTileLink";
	public static readonly string PREFAB_NAME_TILE_DOOR = "PrefabTileDoor";
	public static readonly string PREFAB_NAME_LOOT = "PrefabLoot";
	public static readonly string PREFAB_NAME_NOTE = "PrefabNote";
	public static readonly string PREFAB_NAME_NPC = "Npc";
	public static readonly string PREFAB_NAME_NPC_RENDERER = "NpcRenderer";
	public static readonly string PREFAB_NAME_NPC_BAR = "NpcBar";
	public static readonly string PREFAB_NAME_ITEMINGRID = "PrefabItemInGrid";
	public static readonly string PREFAB_NAME_ITEMINGRID_NB_GROUPED = "PrefabItemInGridNbGrouped";

	
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_BACKGROUND = "SplashScreenBackground";
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_FOREGROUND = "SplashScreenForeground";
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_TITLE = "SplashScreenSplatTitle";
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_CREDITS = "SplashScreenSplatCredits";
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_TITLE = "SplashScreenTitle";
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_SUBTITLE = "SplashScreenSubTitle";
	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_CREDITS = "SplashScreenCredits";
	public static readonly string GAME_OBJECT_NAME_BUTTON_CONTINUE_GAME = "ButtonContinueGame";
	public static readonly string GAME_OBJECT_NAME_BUTTON_NEW_GAME = "ButtonNewGame";
	public static readonly string GAME_OBJECT_NAME_BUTTON_CREDITS = "ButtonCredits";

	public static readonly string GAME_OBJECT_NAME_SPLASHSCREEN_MANAGER = "SplashScreenManager";
	public static readonly string GAME_OBJECT_NAME_LEVEL_MANAGER = "LevelManager";
	public static readonly string GAME_OBJECT_NAME_INPUTS_MANAGER = "InputsManager";

	public static readonly string GAME_OBJECT_NAME_MAIN_CAMERA = "MainCamera";
	public static readonly string GAME_OBJECT_NAME_HUD = "CanvasHUD";
	public static readonly string GAME_OBJECT_NAME_HUD_HEALTH_BAR = "HUDBarHealth";
	public static readonly string GAME_OBJECT_NAME_HUD_STAMINA_BAR = "HUDBarStamina";
	public static readonly string GAME_OBJECT_NAME_PLAYER = "Player";
	public static readonly string GAME_OBJECT_NAME_PLAYER_RENDERER = "PlayerRenderer";

	public static readonly string GAME_OBJECT_NAME_MAP = "Map";
	public static readonly string GAME_OBJECT_NAME_GROUND = "Ground";
	public static readonly string GAME_OBJECT_NAME_WALL = "Wall";
	public static readonly string GAME_OBJECT_NAME_HUB = "Hub";
	public static readonly string GAME_OBJECT_NAME_LINK = "Link";
	public static readonly string GAME_OBJECT_NAME_DOOR = "Door";
	public static readonly string GAME_OBJECT_NAME_LOOT = "Loot";
	public static readonly string GAME_OBJECT_NAME_NOTE = "Note";
	public static readonly string GAME_OBJECT_NAME_NPC = "Npc";
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
	public static readonly string GAME_OBJECT_NAME_MENU_CURSOR = "MenuCursor";

	public static readonly string GAME_OBJECT_NAME_SUB_MENU_KEEPER = "SubMenuKeeper";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_CHARACTER_STATUS = "SubMenuTypeCharacterStatus";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_GAME_OPTIONS = "SubMenuTypeGameOptions";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT = "SubMenuTypeInventoryManagement";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_ITEMS_CHEST = "SubMenuTypeItemsChest";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_LEVEL_UPGRADE = "SubMenuTypeLevelUpgrade";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_MERCHANT = "SubMenuTypeMerchant";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_SPECIAL_ITEMS_LIST = "SubMenuTypeSpecialItemsList";
	public static readonly string GAME_OBJECT_NAME_SUB_MENU_TELEPORT = "SubMenuTypeTeleport";

	public static readonly string GAME_OBJECT_NAME_GRID_COLLECTIBLE_ITEM = "GridCollectibleItem";
	public static readonly string GAME_OBJECT_NAME_GRID_BAG = "GridBag";
	public static readonly string GAME_OBJECT_NAME_GRID_OBJECTS = "GridObjects";
	public static readonly string GAME_OBJECT_NAME_GRID_HEALS = "GridHeals";
	public static readonly string GAME_OBJECT_NAME_GRID_WEAPONS_LEFT = "GridWeaponsLeft";
	public static readonly string GAME_OBJECT_NAME_GRID_EQUIP = "GridEquip";
	public static readonly string GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT = "GridWeaponsRight";
	public static readonly string GAME_OBJECT_NAME_GRID_DATA = "GridData";
	public static readonly string GAME_OBJECT_NAME_GRID_KEYS = "GridKeys";
	public static readonly string GAME_OBJECT_NAME_GRID_GOALS = "GridGoals";


	public static readonly string SORTING_LAYER_NAME_GROUND = "ground";
	public static readonly string SORTING_LAYER_NAME_WALLS = "walls";
	public static readonly string SORTING_LAYER_NAME_OBJECTS = "objects";
	public static readonly string SORTING_LAYER_NAME_CHARACTERS = "characters";
	public static readonly string SORTING_LAYER_NAME_HUB = "hub";

	public static readonly string FIRST_LEVEL_NAME = "Part1.Laboratory1";//the very first level

	public static readonly string ITEM_ID_CLAWS = "W_CLAWS";
	public static readonly string ITEM_ID_REGENERATION = "H_REGENERATION";

	public enum SceneIndex {
		SCENE_INDEX_SPLASHSCREEN = 0,
		SCENE_INDEX_QUOTE = 1,
		SCENE_INDEX_LEVEL = 2
	}
	
	public static int vectorToAngle(float x, float y) {
		
		if(y == 0) {
			return (x > 0) ? 90 : -90;
		}
		
		return (int)(Mathf.Atan2(x, y) * Mathf.Rad2Deg);
	}
	
	public static Vector2 angleToVector(float angleDegrees, int force) {
		
		float angleRad = angleDegrees * Mathf.Deg2Rad;
		
		return new Vector2(Mathf.Sin(angleRad) * force, Mathf.Cos(angleRad) * force);
	}

	
	public static string defaultLocalization = "fr";//"en-US";

	private static bool isTranslatorInitialized = false;

	private static void initTranslator() {

		//set the language
		List<SmartCultureInfo> supportedLanguages = LanguageManager.Instance.GetSupportedLanguages();
		
		SmartCultureInfo chosenSmartCultureInfo = supportedLanguages[0];
		foreach (SmartCultureInfo info in supportedLanguages) {
			if(defaultLocalization.Equals(info.languageCode)) {
				chosenSmartCultureInfo = info;
				break;
			}
		}
		
		LanguageManager.Instance.ChangeLanguage(chosenSmartCultureInfo);
		
		isTranslatorInitialized = true;
	}


	public static string tr(string key) {

		if(!isTranslatorInitialized) {
			initTranslator();
		}

		return LanguageManager.Instance.GetTextValue(key);
	}


	//init at the end so that all the other constants are available
	public static readonly SubMenuTypeInventoryManagement SUB_MENU_TYPE_INVENTORY_MANAGEMENT = new SubMenuTypeInventoryManagement();
	public static readonly SubMenuTypeSpecialItemsList SUB_MENU_TYPE_SPECIAL_ITEMS_LIST = new SubMenuTypeSpecialItemsList();
	public static readonly MenuTypeInventory MENU_TYPE_INVENTORY = new MenuTypeInventory();

}


