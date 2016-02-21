using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Level;
using System;


public class LevelManager : MonoBehaviour {

	private static string nextLevelName;//used to keep the information between levels
	private static BaseNodeElement lastNodeElementTrigger;//last trigger, spawn, link => used to keep the information between levels
	private static bool mustSpawnPlayerAtHub = false;

	private static string currentLevelName;
	private static NodeLevel currentNodeLevel;//optional

	private bool isRunningSaverLoop = false;
	private Coroutine coroutineSaveLoop;


	void Start () {
		
		if(Application.loadedLevel != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL)) {
			return;
		}

		if(!Debug.isDebugBuild) {
			return;
		}

		if(currentLevelName != null) {
			return;
		}

		//load the current level
		if(!GameSaver.Instance.loadCurrentLevel()) {

			//if no saved data, load the very first level				
			loadNextLevel(Constants.FIRST_LEVEL_NAME);
		}

	}


	void OnLevelWasLoaded(int level) {

		if(Application.loadedLevel != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL)) {
			return;
		}

		if(!isAboutToLoadNextLevel()) {
			return;
		}

		//loaded set the current name
		currentLevelName = nextLevelName;
		currentNodeLevel = null;
		nextLevelName = null;
		
		Debug.Log("LOAD LEVEL : " + currentLevelName + " - SCENE " + level);

		createLevel();
		createMap();
		createGameElements();

		initPlayerStats();
		spawnPlayer();

		//free for further level load
		lastNodeElementTrigger = null;
		mustSpawnPlayerAtHub = false;

		//save data to keep state as the player is in another changed level
		GameSaver.Instance.saveCurrentLevel();
		GameSaver.Instance.savePlayer();
		GameSaver.Instance.saveNpcs();
		GameSaver.Instance.saveAllToFile();

		//load listener events after all other loaded elements
		GameSaver.Instance.loadListenerEvents();

		startSaverCoroutine();
	}

	private void createLevel() {

		TextAsset textAssetLevel = GameHelper.Instance.loadLevelAsset(currentLevelName);
		if(textAssetLevel == null) {
			Debug.LogWarning("Could not load textAssetLevel : " + currentLevelName);
			return;
		}

		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAssetLevel.text);
		
		XmlElement rootNode = xmlDocument.DocumentElement;

		currentNodeLevel = new NodeLevel(rootNode.SelectSingleNode("node"));


		/*
		/// TODO DEBUG ///
		if(currentNodeLevel.spawnElement == null) {
			Debug.Log(">>> nodeLevel.spawnElement => null");
		} else {	
			Debug.Log(">>> nodeLevel.spawnElement => " + 
			          "x(" + currentNodeLevel.spawnElement.nodePosition.x + ") " +
			          "y(" + currentNodeLevel.spawnElement.nodePosition.y + ") " +
			          "direction(" + currentNodeLevel.spawnElement.nodeDirection.value + ")");
		}
		
		for(int i=0;i<currentNodeLevel.getHubCount();i++) {
			
			NodeElementHub hubElement = currentNodeLevel.getHub(i);
			
			Debug.Log(">>> nodeLevel.hubElement[" + i + "] => " + 
			          "x(" + hubElement.nodePosition.x + ") " +
			          "y(" + hubElement.nodePosition.y + ") " +
			          "direction(" + hubElement.nodeDirection.value + ")");
		}

		for(int i=0;i<currentNodeLevel.getLinkCount();i++) {

			NodeElementLink linkElement = currentNodeLevel.getLink(i);

			Debug.Log(">>> nodeLevel.linkElement[" + i + "] => " + 
			          "x(" + linkElement.nodePosition.x + ") " +
			          "y(" + linkElement.nodePosition.y + ") " +
			          "nextMap(" + linkElement.nodeNextMap.value + ") " +
			          "nextPos.x(" + linkElement.nodeNextPosition.x + ") " +
			          "nextPos.y(" + linkElement.nodeNextPosition.y + ") " +
					  "nextDirection(" + linkElement.nodeNextDirection.value + ")");
		}*/
		
		/*   
		   ////TODO TEST
		   foreach (XmlNode node in mainNode) {
			
			if("pos".Equals(LevelNode.getText(node))) {
				
				NodePosition n = new NodePosition(node);
				Debug.Log(">>> DONE !!! => pos : " + n.x + " / " + n.y);
				
			} else if("level".Equals(LevelNode.getText(node))) {
				
				LevelNodeInt n = new LevelNodeInt(node);
				Debug.Log(">>> DONE !!! => level : " + n.value);
				
			} else if("name".Equals(LevelNode.getText(node))) {
				
				LevelNodeString n = new LevelNodeString(node);
				Debug.Log(">>> DONE !!! => name : " + n.value);
				
			} else if("label".Equals(LevelNode.getText(node))) {
				
				LevelNodeLabel n = new LevelNodeLabel(node);
				Debug.Log(">>> DONE !!! => label : " + n.value);
			}*/
		/// TODO DEBUG ///

	}


	private void createMap() {
		
		TextAsset textAssetMap = GameHelper.Instance.loadMapAsset(currentLevelName);
		if(textAssetMap == null) {
			throw new System.InvalidOperationException("Could not load textAssetMap : " + currentLevelName);
		}

		Dictionary<string,object> dict = Json.Deserialize(textAssetMap.text) as Dictionary<string,object>;

		//generate map
		TiledMap.Map map = new TiledMap.Map(dict);
		map.instanciateMap(GameHelper.Instance.getMapGameObject());

	}

	private void createGameElements() {

		if(currentNodeLevel == null) {
			return;
		}

		// load hub if there is one
		if(currentNodeLevel.hubElement != null) {

			HubCreator hubCreator = new HubCreator();
			hubCreator.createNewGameObject(currentNodeLevel.hubElement);

			//init
			GameSaver.Instance.loadHub();
		}


		// load links
		LinkCreator linkCreator = new LinkCreator();

		int linkCount = currentNodeLevel.getLinkCount();
		for(int i=0 ; i<linkCount ; i++) {

			linkCreator.createNewGameObject(currentNodeLevel.getLink(i));
		}

		
		// load doors
		DoorCreator doorCreator = new DoorCreator();
		
		int doorCount = currentNodeLevel.getDoorCount();
		for(int i=0 ; i<doorCount ; i++) {
			
			doorCreator.createNewGameObject(currentNodeLevel.getDoor(i));
		}
		//init
		GameSaver.Instance.loadDoors();


		// load loots
		LootCreator lootCreator = new LootCreator();

		Dictionary<string, LootSaveData> lootsSaveDataById = GameSaver.Instance.getLootsSaveData();
		
		int lootCount = currentNodeLevel.getLootCount();
		for(int i=0 ; i<lootCount ; i++) {

			NodeElementLoot nodeElementLoot = currentNodeLevel.getLoot(i);
			string elementId = nodeElementLoot.nodeId.value;

			bool isCollected = false;
			LootSaveData lootSaveData = null;

			if(lootsSaveDataById != null) {
				lootSaveData = lootsSaveDataById[elementId];
				isCollected = lootSaveData.isCollected;
			}

			// create loot only if the loot was not collected
			if(!isCollected) {

				GameObject gameObjectLoot = lootCreator.createNewGameObject(nodeElementLoot);
				
				//init if previously saved
				if(lootSaveData != null) {
					lootSaveData.assign(gameObjectLoot.GetComponent<Loot>());
				}
			}


		}


		// load NPCs
		NpcCreator npcCreator = new NpcCreator();
		
		int npcCount = currentNodeLevel.getNpcCount();
		for(int i=0 ; i<npcCount ; i++) {
			
			npcCreator.createNewGameObject(currentNodeLevel.getNpc(i));
		}
		//init
		GameSaver.Instance.loadNpcs();

	}

	private void spawnPlayer() {

		BaseNodeElement currentNodeElementTrigger;

		if(mustSpawnPlayerAtHub) {
			//the player has died, the current node is the player
			currentNodeElementTrigger = currentNodeLevel.hubElement;

		} else { 

			bool hasLoadedPlayer = false;

			//load saved player
			if(GameSaver.Instance.loadPlayer()) {
				hasLoadedPlayer = true;
			}

			if(lastNodeElementTrigger != null) {
				//something has triggered the load level (can be a link or a spawn)
				currentNodeElementTrigger = lastNodeElementTrigger;

			} else { 

				if(hasLoadedPlayer) {
					//done
					return;
				}

				if(currentNodeLevel == null) {
					//no level file provided : replace by default spawn element
					currentNodeElementTrigger = new NodeElementSpawn();
					
				} else {
					
					//get spawn element of the level
					currentNodeElementTrigger = currentNodeLevel.spawnElement;
					if(currentNodeElementTrigger == null) {
						//replace by default spawn element
						currentNodeElementTrigger = new NodeElementSpawn();
					}
				}
			}
		}

		processPlayerLoad(currentNodeElementTrigger);

	}
	
	public void loadNextLevel(string levelName) {
		
		if(isAboutToLoadNextLevel()) {
			//call already done, waiting for level to load
			return;
		}
		if (AutoFade.Fading) {
			//already loading
			return;
		}
		
		nextLevelName = levelName;

		stopSaverCoroutine();

		bool hasCurrentLevel = !string.IsNullOrEmpty(currentLevelName);

		if(hasCurrentLevel) {
			//the current level is not currently loading

			GameHelper.Instance.getPlayer().disableControls();
			
			GameSaver.Instance.savePlayer();
			GameSaver.Instance.saveNpcs();
			GameSaver.Instance.saveAllToFile();
		}

		bool hasFade = (hasCurrentLevel || !Debug.isDebugBuild || Application.loadedLevel != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL));

		//no fadein if level is the first
		AutoFade.LoadLevel((int)(Constants.SceneIndex.SCENE_INDEX_LEVEL), hasFade ? 0.3f : 0, 0.3f, Color.black);
	}
	
	private bool isAboutToLoadNextLevel() {
		return !(string.IsNullOrEmpty(nextLevelName));
	}

	public void processPlayerLoad(BaseNodeElement nodeElement) {
		//move player in level with SPAWN / HUB / LINK / load from save

		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		NodePosition nodePosition = null;
		NodeDirection nodeDirection = null;

		if(nodeElement is NodeElementSpawn) {

			NodeElementSpawn nodeElementSpawn = nodeElement as NodeElementSpawn;
			
			nodePosition = nodeElementSpawn.nodePosition;
			nodeDirection = nodeElementSpawn.nodeDirection;

		} else if(nodeElement is NodeElementLink) {
			
			NodeElementLink nodeElementLink = nodeElement as NodeElementLink;
			
			nodePosition = nodeElementLink.nodeNextPosition;
			nodeDirection = nodeElementLink.nodeNextDirection;
			
		} else if(nodeElement is NodeElementHub) {

			NodeElementHub nodeElementHub = nodeElement as NodeElementHub;
			
			nodePosition = nodeElementHub.nodePosition;
			nodeDirection = nodeElementHub.nodeSpawnDirection;
			
		} else {

			throw new System.NotSupportedException("Not supported yet");
		}

		GameHelper.Instance.getPlayer().setInitialPosition(nodePosition, nodeDirection);

	}

	private void initPlayerStats() {
		
		Player player = GameHelper.Instance.getPlayer();

		//load player stats
		if(!GameSaver.Instance.loadPlayerStats()) {
			//very first init of the player stats
			player.initStats(5, 5);
		
		} else if(mustSpawnPlayerAtHub) {

			player.reinitLifeAndStamina();
		}

	}

	
	public void preparePlayerToRespawn() {
		
		if(isAboutToLoadNextLevel()) {
			//already processing for next level
			return;
		}

		stopSaverCoroutine();

		//save player so that when the player quit and restart the game, the player respawns
		GameSaver.Instance.savePlayer();
		GameSaver.Instance.saveAllToFile();
	}

	public void processPlayerRespawn() {
		
		if(isAboutToLoadNextLevel()) {
			//not processing for next level
			return;
		}
		
		//load next level with HUB
		
		string requiredLevelName = GameHelper.Instance.getPlayer().levelNameForLastHub;
		
		if(string.IsNullOrEmpty(requiredLevelName)) {
			throw new System.InvalidOperationException("levelNameForlastHub is empty");
		}
		
		mustSpawnPlayerAtHub = true;

		//load level
		loadNextLevel(requiredLevelName);

	}

	public void processLink(Link link) {

		if(isAboutToLoadNextLevel()) {
			//already processing for next level
			return;
		}

		//load next level with LINK

		NodeElementLink nodeElementLink = link.nodeElementLink;

		string requiredLevelName = null;
		NodeString nodeNextMap = nodeElementLink.nodeNextMap;
		if(nodeNextMap != null) {
			requiredLevelName = nodeNextMap.value;
		} else {
			requiredLevelName = currentLevelName;
		}

		if(string.IsNullOrEmpty(requiredLevelName)) {
			throw new System.InvalidOperationException("The requiredLevelName is empty");
		}

		lastNodeElementTrigger = nodeElementLink;

		//load level
		loadNextLevel(requiredLevelName);

	}


	public string getCurrentLevelName() {
		return currentLevelName;
	}
	
	private void startSaverCoroutine() {
		
		stopSaverCoroutine();
		
		coroutineSaveLoop = StartCoroutine(runSaverLoop());
	}
	
	private void stopSaverCoroutine() {
		
		if(coroutineSaveLoop != null) {
			StopCoroutine(coroutineSaveLoop);
		}
	}
	
	//save the game every x seconds
	private IEnumerator runSaverLoop() {
		
		isRunningSaverLoop = true;
		
		while(isRunningSaverLoop) {
			
			yield return new WaitForSeconds(10);
			
			GameSaver.Instance.savePlayer();
			GameSaver.Instance.saveNpcs();
			GameSaver.Instance.saveDoors();
			GameSaver.Instance.saveLoots();
			GameSaver.Instance.saveAllToFile();

			Debug.Log("[GAME SAVED " + DateTime.Now + "]");
		}
		
	}

}
