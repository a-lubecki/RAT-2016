using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

public class GameHelper {
	
	private static GameHelper instance;
	
	private GameHelper() {}
	
	public static GameHelper Instance {

		get {
			if (instance == null) {
				instance = new GameHelper();
			}
			return instance;
		}
	}


	public GameObject getMainCamera() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_MAIN_CAMERA);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		return gameObject;
	}

	public CameraResizer getMainCameraResizer() {

		return getMainCamera().GetComponent<CameraResizer>();
	}

	public GameObject getHUD() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_HUD);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		return gameObject;
	}

	public GameObject getHUDHealthBar() {
		
		Transform transform = getHUD().transform.Find(Constants.GAME_OBJECT_NAME_HUD_HEALTH_BAR);
		if(transform == null) {
			throw new System.InvalidOperationException();
		}
		
		return transform.gameObject;
	}
	
	public GameObject getHUDStaminaBar() {
		
		Transform transform = getHUD().transform.Find(Constants.GAME_OBJECT_NAME_HUD_STAMINA_BAR);
		if(transform == null) {
			throw new System.InvalidOperationException();
		}
		
		return transform.gameObject;
	}

	public GameObject getSubMenuKeeper() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_KEEPER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		return gameObject;
	}

	public SplashScreenManager getSplashScreenManager() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_MANAGER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		SplashScreenManager component = gameObject.GetComponent<SplashScreenManager>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}
		
		return component;
	}

	public LevelManager getLevelManager() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_LEVEL_MANAGER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		LevelManager component = gameObject.GetComponent<LevelManager>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}
		
		return component;
	}

	public InputsManager getInputsManager() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_INPUTS_MANAGER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		InputsManager component = gameObject.GetComponent<InputsManager>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}
		
		return component; 
	}

	public MessageDisplayer getMessageDisplayer() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_MESSAGE_DISPLAYER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}

		MessageDisplayer component = gameObject.GetComponent<MessageDisplayer>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}

		return component;
	}

	public GameObject getMapGameObject() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_MAP);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		return gameObject;
	}

	public GameObject getForegroundGlassGameObject() {

		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_IMAGE_FOREGROUND_GLASS);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}

		return gameObject;
	}

	public GameObject getMenuGameObject() {

		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_MENU);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		return gameObject;
	}

	public Menu getMenu() {
		
		Menu component = getMenuGameObject().GetComponent<Menu>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}

		return component;
	}

	public GameObject getMenuArrowLeft() {

		Transform transform = getMenuGameObject().transform.Find(Constants.GAME_OBJECT_NAME_MENU_ARROW_LEFT);
		if(transform == null) {
			throw new System.InvalidOperationException();
		}

		return transform.gameObject;
	}

	public GameObject getMenuArrowRight() {

		Transform transform = getMenuGameObject().transform.Find(Constants.GAME_OBJECT_NAME_MENU_ARROW_RIGHT);
		if(transform == null) {
			throw new System.InvalidOperationException();
		}

		return transform.gameObject;
	}

	public MenuCursorBehavior getMenuCursorBehavior() {
		
		Transform transform = getMenuGameObject().transform.Find(Constants.GAME_OBJECT_NAME_MENU_CURSOR);
		if(transform == null) {
			throw new System.InvalidOperationException();
		}

		MenuCursorBehavior component = transform.GetComponent<MenuCursorBehavior>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}

		return component;
	}

	public ItemInGridBehavior getItemInGridBehavior(ItemInGrid itemInGrid) {

		InventoryGrid grid = getMenu().getCurrentSubMenuType().findInventoryGrid(itemInGrid.getGridName());
		if(grid == null) {
			return null;
		}

		ItemInGridBehavior[] itemInGridBehaviors = grid.transform.GetComponentsInChildren<ItemInGridBehavior>();

		foreach(ItemInGridBehavior itemInGridBehavior in itemInGridBehaviors) {
			if(itemInGridBehavior.itemInGrid == itemInGrid) {
				return itemInGridBehavior;
			}
		}

		return null;
	}

	
	public Hub getHub() {
		return getLevelManager().hub;
	}

	public HubBehavior findHubBehavior() {
		return GameObject.FindObjectOfType<HubBehavior>();//can be null
	}

	public Door[] getDoors() {
		return getLevelManager().doors;
	}

	public DoorBehavior findDoorBehavior(Door door) {

		foreach(DoorBehavior doorBehavior in GameObject.FindObjectsOfType<DoorBehavior>()) {
			if(doorBehavior.door == door) {
				return doorBehavior;
			}
		}

		return null;//not found
	}

	public Loot[] getLoots() {
		return getLevelManager().loots;
	}

	public LootBehavior findLootBehavior(Loot loot) {

		foreach(LootBehavior lootBehavior in GameObject.FindObjectsOfType<LootBehavior>()) {
			if(lootBehavior.loot == loot) {
				return lootBehavior;
			}
		}

		return null;//not found
	}

	public Npc[] getNpcs() {
		return getLevelManager().npcs;
	}

	public NpcBehavior findNpcBehavior(Npc npc) {

		foreach(NpcBehavior npcBehavior in GameObject.FindObjectsOfType<NpcBehavior>()) {
			if(npcBehavior.npc == npc) {
				return npcBehavior;
			}
		}

		return null;//not found
	}

	public NpcRendererBehavior findNpcRendererBehavior(Npc npc) {

		foreach(NpcRendererBehavior npcRendererBehavior in GameObject.FindObjectsOfType<NpcRendererBehavior>()) {
			if(npcRendererBehavior.npc == npc) {
				return npcRendererBehavior;
			}
		}

		return null;//not found
	}

	public Player getPlayer() {
		return getLevelManager().player;
	}
	
	public PlayerBehavior findPlayerBehavior() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		PlayerBehavior component = gameObject.GetComponent<PlayerBehavior>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}
		
		return component;
	}

	public PlayerRendererBehavior findPlayerRendererBehavior() {

		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER_RENDERER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}

		PlayerRendererBehavior component = gameObject.GetComponent<PlayerRendererBehavior>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}

		return component;
	}

	public XpDisplayManager getXpDisplayManager() {
				
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_XP_DISPLAY_MANAGER);
		if(gameObject == null) {
			return null;
		}
		
		return gameObject.GetComponent<XpDisplayManager>();
	}

	public Texture2D loadTexture2DAsset(string imagePath) {
		
		if(string.IsNullOrEmpty(imagePath)) {
			return null;
		}

		Texture2D texture = Resources.Load<Texture2D>(imagePath) as Texture2D;
		if(texture == null) {
			throw new System.InvalidOperationException("Could not load image asset : " + imagePath);
		}

		texture.filterMode = FilterMode.Point;

		return texture;
	}
	
	public Sprite loadSpriteAsset(string imagePath) {

		if(string.IsNullOrEmpty(imagePath)) {
			return null;
		}
		
		Sprite sprite = Resources.Load<Sprite>(imagePath) as Sprite;
		if(sprite == null) {
			throw new System.InvalidOperationException("Could not load image asset : " + imagePath);
		}

		return sprite;
	}

	public Sprite loadMultiSpriteAsset(string imagePath, string spriteName) {
		
		if(string.IsNullOrEmpty(imagePath)) {
			return null;
		}
		if(string.IsNullOrEmpty(spriteName)) {
			return null;
		}

		UnityEngine.Object[] sprites = Resources.LoadAll(imagePath);
		if(sprites == null || sprites.Length <= 0) {
			throw new System.InvalidOperationException("Could not load multi image asset : " + imagePath);
		}

		foreach(UnityEngine.Object o in sprites) {

			if(!(o is Sprite)) {
				continue;
			}

			Sprite s = o as Sprite;
			if(spriteName.Equals(s.name)) {
				return s;
			}
		}

		throw new System.InvalidOperationException("Could not load image asset : " + imagePath + " => " + spriteName);
	}

	public TextAsset loadTextAsset(string path) {

		if(string.IsNullOrEmpty(path)) {
			return null;
		}
		return Resources.Load<TextAsset>(path) as TextAsset;
	}

	public TextAsset loadLevelAsset(string levelName) {

		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}
		return loadTextAsset(Constants.PATH_RES_MAPS + "Level." + levelName);
	}
	
	public TextAsset loadMapAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}
		return loadTextAsset(Constants.PATH_RES_MAPS + "Map." + levelName);
	}

	public GameObject loadPrefabAsset(string prefabName) {
		
		if(string.IsNullOrEmpty(prefabName)) {
			return null;
		}
		return Resources.Load<GameObject>(Constants.PATH_RES_PREFABS + prefabName) as GameObject;
	}


	public Vector2 newPositionOnMap(int x, int y) {
		return new Vector2(x * Constants.TILE_SIZE, - y * Constants.TILE_SIZE);
	}

	public GameObject newGameObjectFromPrefab(GameObject prefab) {
		return newGameObjectFromPrefab(prefab, 0, 0);
	}

	public GameObject newGameObjectFromPrefab(GameObject prefab, int x, int y) {
		return newGameObjectFromPrefab(prefab, x, y, Quaternion.identity);
	}

	public GameObject newGameObjectFromPrefab(GameObject prefab, int x, int y, Quaternion rotation) {

		if(prefab == null) {
			throw new System.ArgumentException();
		}

		return GameObject.Instantiate(
			prefab, 
			newPositionOnMap(x, y), 
			rotation) as GameObject;
	}

	
	public IMapListener getCurrentMapListener() {

		MonoBehaviour mapListenerBehaviour = getCurrentMapListenerBehaviour();

		if(mapListenerBehaviour is IMapListener) {
			return (IMapListener) mapListenerBehaviour;
		}

		return null;
	}

	public MonoBehaviour getCurrentMapListenerBehaviour() {

		LevelManager levelManager = getLevelManager();

		MonoBehaviour mapListener = levelManager.GetComponent<IMapListener>() as MonoBehaviour;

		if(mapListener == null) {
			//create the listener class for this map
			string levelName = GameManager.Instance.getCurrentLevelName();
			if(string.IsNullOrEmpty(levelName)) {
				Debug.LogWarning("Level name is or empty");
				return null;
			}

			string className = "MapListener_" + Regex.Replace(levelName, "\\.", "_");

			Type listenerClassType = Type.GetType(className, false, false);

			if(listenerClassType == null) {
				Debug.LogWarning("Class was not loaded : " + className);
				return null;
			}

			//add it
			mapListener = levelManager.gameObject.AddComponent(listenerClassType) as MonoBehaviour;
		}

		return mapListener;
	} 
}
