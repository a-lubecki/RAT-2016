using UnityEngine;
using UnityEngine.SceneManagement;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Node;
using System;


public class LevelManager : MonoBehaviour {

	private static string nextLevelName;//used to keep the information between levels
	private static BaseNodeElement lastNodeElementTrigger;//last trigger, spawn, link => used to keep the information between levels
	private static bool mustSpawnPlayerAtHub = false;

	private bool isVeryFirstStart = false;

	private bool isRunningSaverLoop = false;
	private Coroutine coroutineSaveLoop;

	void Start () {
		
		if(SceneManager.GetActiveScene().buildIndex != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL)) {
			return;
		}

		if(!Debug.isDebugBuild) {
			return;
		}

		if(GameManager.Instance.hasCurrentLevel()) {
			return;
		}

		GameManager.Instance.loadNodeGame();

		//load the current level
		string levelName = GameSaver.Instance.getCurrentLevelName();
		if(levelName == null) {

			//if no saved data, load the very first level				
			isVeryFirstStart = true;
			levelName = Constants.FIRST_LEVEL_NAME;
		}

		loadNextLevel(levelName);

	}


	void OnLevelWasLoaded(int level) {

		if(SceneManager.GetActiveScene().buildIndex != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL)) {
			return;
		}

		if(!isAboutToLoadNextLevel()) {
			return;
		}

		if(isVeryFirstStart) {
			MessageDisplayer.Instance.displayMessages(new Message(this, "Nouvelle objectif : Sortir du complexe"));//TODO TEST
		}

		//loaded set the current name
		GameManager.Instance.loadNodeLevel(nextLevelName);

		if(!GameManager.Instance.hasCurrentLevel()) {
			Debug.Log("LEVEL NOT LOADED : " + nextLevelName + " - SCENE " + level);
			return;
		}

		nextLevelName = null;
		
		Debug.Log("LOAD LEVEL : " + GameManager.Instance.getCurrentLevelName() + " - SCENE " + level);

		createMap();
		createGameElements();
		spawnPlayer();

		//free for further level load
		lastNodeElementTrigger = null;
		mustSpawnPlayerAtHub = false;

		//save data to keep state as the player is in another changed level
		GameSaver.Instance.saveCurrentLevel();
		GameManager.Instance.saveGame(false);

		//load listener events after all other loaded elements
		IMapListener mapListener = GameHelper.Instance.getCurrentMapListener();
		if(mapListener != null) {
			
			List<ListenerEventSaveData> listenerEventSaveDataList = GameSaver.Instance.getListenerEvents();
			if(listenerEventSaveDataList != null) {
				
				foreach(ListenerEventSaveData eventData in listenerEventSaveDataList) {
					
					if(eventData.getIsAchieved()) {
						eventData.assign(mapListener);
					}
				}
			}

		}

		startSaverCoroutine();

		isVeryFirstStart = false;
	}

	private void createMap() {

		string currentLevelName = GameManager.Instance.getCurrentLevelName();

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

		NodeLevel currentNodeLevel = GameManager.Instance.getCurrentNodeLevel();

		// load hub if there is one
		if(currentNodeLevel.hubElement != null) {
			
			HubSaveData hubSaveData = GameSaver.Instance.getHubSaveData();

			HubCreator hubCreator = new HubCreator();
			GameObject gameObjectHub = hubCreator.createNewGameObject(currentNodeLevel.hubElement);

			//init if previously saved
			if(hubSaveData != null) {
				hubSaveData.assign(gameObjectHub.GetComponent<Hub>());
			}

		}


		// load links
		LinkCreator linkCreator = new LinkCreator();

		int linkCount = currentNodeLevel.getLinkCount();
		for(int i=0 ; i<linkCount ; i++) {

			linkCreator.createNewGameObject(currentNodeLevel.getLink(i));
		}

		
		// load doors
		DoorCreator doorCreator = new DoorCreator();
		
		Dictionary<string, DoorSaveData> doorsSaveDataById = GameSaver.Instance.getDoorsSaveData();

		int doorCount = currentNodeLevel.getDoorCount();
		for(int i=0 ; i<doorCount ; i++) {

			NodeElementDoor nodeElementDoor = currentNodeLevel.getDoor(i);
			string elementId = nodeElementDoor.nodeId.value;
			
			GameObject gameObjectDoor = doorCreator.createNewGameObject(nodeElementDoor);

			//init if previously saved
			if(doorsSaveDataById != null && doorsSaveDataById.ContainsKey(elementId)) {
				DoorSaveData doorSaveData = doorsSaveDataById[elementId];
				doorSaveData.assign(gameObjectDoor.GetComponent<Door>());
			}
		}


		// load loots
		LootCreator lootCreator = new LootCreator();

		Dictionary<string, LootSaveData> lootsSaveDataById = GameSaver.Instance.getLootsSaveData();
		
		int lootCount = currentNodeLevel.getLootCount();
		for(int i=0 ; i<lootCount ; i++) {

			NodeElementLoot nodeElementLoot = currentNodeLevel.getLoot(i);
			string elementId = nodeElementLoot.nodeId.value;

			bool isCollected = false;
			LootSaveData lootSaveData = null;

			if(lootsSaveDataById != null && lootsSaveDataById.ContainsKey(elementId)) {
				lootSaveData = lootsSaveDataById[elementId];
				isCollected = lootSaveData.getIsCollected();
			}

			// create loot only if the loot was not collected
			if(!isCollected) {
				lootCreator.createNewGameObject(nodeElementLoot);
			}


		}


		// load NPCs
		NpcCreator npcCreator = new NpcCreator();

		Dictionary<string, NpcSaveData> npcsSaveDataById = GameSaver.Instance.getNpcsSaveData();

		int npcCount = currentNodeLevel.getNpcCount();
		for(int i=0 ; i<npcCount ; i++) {
			
			NodeElementNpc nodeElementNpc = currentNodeLevel.getNpc(i);
			string elementId = nodeElementNpc.nodeId.value;

			bool isDead = false;
			NpcSaveData npcSaveData = null;
			
			//init if previously saved
			if(npcsSaveDataById != null && npcsSaveDataById.ContainsKey(elementId)) {
				npcSaveData = npcsSaveDataById[elementId];
				isDead = (npcSaveData.getCurrentLife() <= 0);
			}

			if(!isDead) {

				GameObject gameObjectNpc = npcCreator.createNewGameObject(nodeElementNpc);

				//init if previously saved
				if(npcSaveData != null) {
					npcSaveData.assign(gameObjectNpc.GetComponent<Npc>());
				}

			} else {
				//create body
				
				//TODO TEST !!!
				GameObject gameObjectNpc = npcCreator.createNewGameObject(nodeElementNpc);
				if(npcSaveData != null) {
					npcSaveData.assign(gameObjectNpc.GetComponent<Npc>());
				}
				//TODO TEST !!!
			}
		}

	}

	private void spawnPlayer() {

		Player player = GameHelper.Instance.getPlayer();

		//load player stats
		PlayerStatsSaveData playerStatsSaveData = GameSaver.Instance.getPlayerStatsSaveData();
		if(playerStatsSaveData != null) {

			playerStatsSaveData.assign(player);

			if(mustSpawnPlayerAtHub) {
				player.reinitLifeAndStamina();
			}

		} else {

			//very first init of the player stats
			player.initStats(5, 5);

		}


		NodeLevel currentNodeLevel = GameManager.Instance.getCurrentNodeLevel();

		BaseNodeElement currentNodeElementTrigger;

		if(mustSpawnPlayerAtHub) {
			//the player has died, the current node is the player
			currentNodeElementTrigger = currentNodeLevel.hubElement;

		} else { 

			bool hasLoadedPlayer = false;

			//load saved player
			PlayerSaveData playerSaveData = GameSaver.Instance.getPlayerSaveData();
			if(playerSaveData != null) {
				
				hasLoadedPlayer = true;
				playerSaveData.assign(GameHelper.Instance.getPlayer());
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

		bool hasCurrentLevel = GameManager.Instance.hasCurrentLevel();

		if(hasCurrentLevel) {
			//the current level is not currently loading

			GameHelper.Instance.getPlayer().disableControls();

			//when the user quit the level, the doors, enemies... must be saved to be at this state when the player come back after
			GameManager.Instance.saveGame(false);
		}

		bool hasFade = (hasCurrentLevel || !Debug.isDebugBuild || SceneManager.GetActiveScene().buildIndex != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL));

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

	public void preparePlayerToRespawn() {
		
		if(isAboutToLoadNextLevel()) {
			//already processing for next level
			return;
		}

		stopSaverCoroutine();

		//save player so that when the player can't cheat by quitting and restarting the game to avoid dying
		GameManager.Instance.saveGame(false);
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
			requiredLevelName = GameManager.Instance.getCurrentLevelName();
		}

		if(string.IsNullOrEmpty(requiredLevelName)) {
			throw new System.InvalidOperationException("The requiredLevelName is empty");
		}

		lastNodeElementTrigger = nodeElementLink;

		//load level
		loadNextLevel(requiredLevelName);

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

			GameManager.Instance.saveGame(true);

			Debug.Log("[GAME SAVED " + DateTime.Now + "]");
		}
		
	}

}
