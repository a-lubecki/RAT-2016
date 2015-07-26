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

	public GameObject getPlayerGameObject() {

		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}

		return gameObject;
	}
	
	public EntityRenderer getPlayerRenderer() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER_RENDERER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		EntityRenderer component = gameObject.GetComponent<EntityRenderer>();
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
	
	public GameObject getMapGameObject() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_MAP);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		return gameObject;
	}
	
	public Hub getHub() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_HUB);
		if(gameObject == null) {
			return null;//no hub
		}
		
		return gameObject.GetComponent<Hub>();
	}

	public Door[] getDoors() {
		return GameObject.FindObjectsOfType<Door>();
	}

	public Npc[] getNpcs() {
		return GameObject.FindObjectsOfType<Npc>();
	}

	public Texture2D loadTexture2DAsset(string imagePath) {

		Texture2D texture = UnityEditor.AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture2D)) as Texture2D;
		if(texture == null) {
			throw new System.InvalidOperationException("Could not load image asset : " + imagePath);
		}

		texture.filterMode = FilterMode.Point;

		return texture;
	}

	public TextAsset loadLevelAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_MAPS + levelName + ".xml", typeof(TextAsset)) as TextAsset;
	}
	
	public TextAsset loadMapAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_MAPS + levelName + ".json", typeof(TextAsset)) as TextAsset;
	}
	
	public GameObject loadPrefabAsset(string prefabName) {
		
		if(string.IsNullOrEmpty(prefabName)) {
			return null;
		}
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_PREFABS + prefabName, typeof(GameObject)) as GameObject;
	}


	public Vector2 newPositionOnMap(int x, int y) {
		return new Vector2(x * Constants.TILE_SIZE, - y * Constants.TILE_SIZE);
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
			string levelName = levelManager.getCurrentLevelName();
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
