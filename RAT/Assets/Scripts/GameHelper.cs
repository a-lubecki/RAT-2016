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

	private static GameObject findGameObject(ref WeakReference reference, string gameObjectName) {
		return findGameObject(ref reference, gameObjectName, null);
	}

	private static GameObject findGameObject(ref WeakReference reference, string gameObjectName, GameObject parentGameObject) {

		//try with the stored ref
		if (reference != null) {

			object target = reference.Target;
			if (target != null) {

				//at this point the GameObject can be non-null but destroyed, there are no ways to know if it is null. The only way is to look for a trown exception from a property of GameObject.
				try {
					bool b = (target as GameObject).activeSelf;//trigger an exception if the game object has been destroyed
					return target as GameObject;

				} catch {
					reference = null;
					Debug.Log("NULL REF : " + gameObjectName);
				}

			}

		}

		//retrieve the game object then store it in the ref

		GameObject gameObject = null;

		if (parentGameObject == null) {
			
			gameObject = GameObject.Find(gameObjectName);

			if(gameObject == null) {
				throw new System.InvalidOperationException();
			}

		} else {
			
			Transform transform = parentGameObject.transform.Find(gameObjectName);

			if(transform == null) {
				throw new System.InvalidOperationException();
			}

			gameObject = transform.gameObject;
		}

		reference = new WeakReference(gameObject);

		return gameObject;
	}

	private static T findComponent<T>(GameObject gameObject) {

		T component = gameObject.GetComponent<T>();
		if (component == null) {
			throw new System.InvalidOperationException();
		}

		return component;
	}

	private WeakReference mainCameraRef;
	private WeakReference HUDRef;
	private WeakReference HUDHealthBarRef;
	private WeakReference HUDStaminaBarRef;
	private WeakReference subMenuKeeperRef;
	private WeakReference splashScreenManagerRef;
	private WeakReference levelManagerRef;
	private WeakReference inputsManagerRef;
	private WeakReference messageDisplayerRef;
	private WeakReference mapRef;
	private WeakReference foregroundGlassRef;
	private WeakReference menuRef;
	private WeakReference menuArrowLeftRef;
	private WeakReference menuArrowRightRef;
	private WeakReference menuCursorRef;
	private WeakReference xpDisplayManagerRef;

	public GameObject getMainCameraGameObject() {
		return findGameObject(ref mainCameraRef, Constants.GAME_OBJECT_NAME_MAIN_CAMERA);
	}

	public CameraResizer getMainCameraResizer() {
		return findComponent<CameraResizer>(
			getMainCameraGameObject()
		);
	}

	public GameObject getHUDGameObject() {
		return findGameObject(ref HUDRef, Constants.GAME_OBJECT_NAME_HUD);
	}

	public GameObject getHUDHealthBarGameObject() {
		return findGameObject(ref HUDHealthBarRef, Constants.GAME_OBJECT_NAME_HUD_HEALTH_BAR, getHUDGameObject());
	}
		
	public GameObject getHUDStaminaBarGameObject() {
		return findGameObject(ref HUDStaminaBarRef, Constants.GAME_OBJECT_NAME_HUD_STAMINA_BAR, getHUDGameObject());
	}

	public GameObject getSubMenuKeeperGameObject() {
		return findGameObject(ref subMenuKeeperRef, Constants.GAME_OBJECT_NAME_SUB_MENU_KEEPER);
	}

	public SplashScreenManager getSplashScreenManager() {
		return findComponent<SplashScreenManager>(
			findGameObject(ref splashScreenManagerRef, Constants.GAME_OBJECT_NAME_SPLASHSCREEN_MANAGER)
		);
	}

	public LevelManager getLevelManager() {
		return findComponent<LevelManager>(
			findGameObject(ref levelManagerRef, Constants.GAME_OBJECT_NAME_LEVEL_MANAGER)
		);
	}

	public InputsManager getInputsManager() {
		return findComponent<InputsManager>(
			findGameObject(ref inputsManagerRef, Constants.GAME_OBJECT_NAME_INPUTS_MANAGER)
		);
	}

	public MessageDisplayer getMessageDisplayer() {
		return findComponent<MessageDisplayer>(
			findGameObject(ref messageDisplayerRef, Constants.GAME_OBJECT_NAME_MESSAGE_DISPLAYER)
		);
	}

	public GameObject getMapGameObject() {
		return findGameObject(ref mapRef, Constants.GAME_OBJECT_NAME_MAP);
	}

	public GameObject getForegroundGlassGameObject() {
		return findGameObject(ref foregroundGlassRef, Constants.GAME_OBJECT_NAME_IMAGE_FOREGROUND_GLASS);
	}

	public GameObject getMenuGameObject() {
		return findGameObject(ref menuRef, Constants.GAME_OBJECT_NAME_MENU);
	}

	public Menu getMenu() {
		return findComponent<Menu>(
			getMenuGameObject()
		);
	}

	public GameObject getMenuArrowLeft() {
		return findGameObject(ref menuArrowLeftRef, Constants.GAME_OBJECT_NAME_MENU_ARROW_LEFT, getMenuGameObject());
	}

	public GameObject getMenuArrowRight() {
		return findGameObject(ref menuArrowRightRef, Constants.GAME_OBJECT_NAME_MENU_ARROW_RIGHT, getMenuGameObject());
	}

	public MenuCursorBehavior getMenuCursorBehavior() {
		return findComponent<MenuCursorBehavior>(
			findGameObject(ref menuCursorRef, Constants.GAME_OBJECT_NAME_MENU_CURSOR)
		);
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

	public Door[] getDoors() {
		return getLevelManager().doors;
	}

	public Loot[] getLoots() {
		return getLevelManager().loots;
	}

	public Npc[] getNpcs() {
		return getLevelManager().npcs;
	}

	public Player getPlayer() {
		return getLevelManager().player;
	}

	public XpDisplayManager getXpDisplayManager() {
		return findComponent<XpDisplayManager>(
			findGameObject(ref xpDisplayManagerRef, Constants.GAME_OBJECT_NAME_XP_DISPLAY_MANAGER)
		);
	}


	private static T loadAsset<T>(string path) where T : UnityEngine.Object {

		if(string.IsNullOrEmpty(path)) {
			return null;
		}

		T asset = Resources.Load<T>(path) as T;
		if(asset == null) {
			throw new System.InvalidOperationException("Could not load " + typeof(T) + " asset : " + path);
		}

		return asset;
	}

	public Texture2D loadTexture2DAsset(string path) {

		Texture2D texture = loadAsset<Texture2D>(path);
		texture.filterMode = FilterMode.Point;

		return texture;
	}
	
	public Sprite loadSpriteAsset(string path) {
		return loadAsset<Sprite>(path);
	}

	public TextAsset loadTextAsset(string path) {
		return loadAsset<TextAsset>(path);
	}

	public TextAsset loadLevelAsset(string levelName) {
		return loadTextAsset(Constants.PATH_RES_MAPS + "Level." + levelName);
	}
	
	public TextAsset loadMapAsset(string levelName) {
		return loadTextAsset(Constants.PATH_RES_MAPS + "Map." + levelName);
	}

	public GameObject loadPrefabAsset(string prefabName) {
		return loadAsset<GameObject>(Constants.PATH_RES_PREFABS + prefabName);
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


	public Vector2 newRealPositionFromMapPosition(int x, int y) {
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
			newRealPositionFromMapPosition(x, y), 
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
