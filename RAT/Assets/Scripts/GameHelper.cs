using System;
using UnityEngine;


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

	
	public PlayerControls getPlayerControls() {

		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER_CONTROLS);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}

		PlayerControls component = gameObject.GetComponent<PlayerControls>();
		if(component == null) {
			throw new System.InvalidOperationException();
		}

		return component;
	}
	
	public CharacterRenderer getPlayerRenderer() {
		
		GameObject gameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_PLAYER_RENDERER);
		if(gameObject == null) {
			throw new System.InvalidOperationException();
		}
		
		CharacterRenderer component = gameObject.GetComponent<CharacterRenderer>();
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


	
	public TextAsset getLevelAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}

		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_MAPS + levelName + ".xml", typeof(TextAsset)) as TextAsset;
	}
	
	public TextAsset getMapAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}

		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_MAPS + levelName + ".json", typeof(TextAsset)) as TextAsset;
	}

}
