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
	private static ISpawnable lastSpawnable;//last trigger, spawn, link => used to keep the information between levels
	private static bool mustSpawnPlayerAtHub = false;

	private bool isVeryFirstStart = false;

	private bool isRunningSaverLoop = false;
	private Coroutine coroutineSaveLoop;

	public Spawn spawn { get; private set; }
	public Link[] links { get; private set; }
	public Door[] doors { get; private set; }
	public Hub hub { get; private set; }
	public Loot[] loots { get; private set; }


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
		lastSpawnable = null;
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
		NodeElementHub nodeElementHub = currentNodeLevel.hubElement;

		// load hub if there is one
		if(nodeElementHub != null) {
			
			HubSaveData hubSaveData = GameSaver.Instance.getHubSaveData();

			bool isActivated = false;

			if(hubSaveData != null) {
				isActivated = hubSaveData.getIsActivated();
			}

			hub = new Hub(nodeElementHub, isActivated);

			new HubCreator().createNewGameObject(nodeElementHub, hub);
		}


		// load links
		LinkCreator linkCreator = new LinkCreator();

		int linksCount = currentNodeLevel.getLinkCount();
		links = new Link[linksCount];

		string currentLevelName = GameManager.Instance.getCurrentLevelName();

		for(int i=0 ; i<linksCount ; i++) {

			NodeElementLink nodeElementLink = currentNodeLevel.getLink(i);

			Link link = new Link(nodeElementLink, currentLevelName);
			links[i] = link;

			linkCreator.createNewGameObject(nodeElementLink, link);

		}

		
		// load doors
		DoorCreator doorCreator = new DoorCreator();
		
		Dictionary<string, DoorSaveData> doorsSaveDataById = GameSaver.Instance.getDoorsSaveData();

		int doorsCount = currentNodeLevel.getDoorCount();
		doors = new Door[doorsCount];

		for(int i=0 ; i<doorsCount ; i++) {

			NodeElementDoor nodeElementDoor = currentNodeLevel.getDoor(i);
			string elementId = nodeElementDoor.nodeId.value;

			bool isOpened;

			if(doorsSaveDataById != null && doorsSaveDataById.ContainsKey(elementId)) {
				DoorSaveData doorSaveData = doorsSaveDataById[elementId];
				isOpened = doorSaveData.getIsOpened();
			} else {
				isOpened = (nodeElementDoor.nodeDoorStatus.value == DoorStatus.OPENED);
			}

			Door door = new Door(nodeElementDoor, isOpened);
			doors[i] = door;

			doorCreator.createNewGameObject(nodeElementDoor, door);

		}


		// load loots
		LootCreator lootCreator = new LootCreator();

		Dictionary<string, LootSaveData> lootsSaveDataById = GameSaver.Instance.getLootsSaveData();

		int lootsCount = currentNodeLevel.getLootCount();
		loots = new Loot[lootsCount];

		for(int i=0 ; i<lootsCount ; i++) {

			NodeElementLoot nodeElementLoot = currentNodeLevel.getLoot(i);
			string elementId = nodeElementLoot.nodeId.value;

			bool isCollected = false;
			LootSaveData lootSaveData = null;

			if(lootsSaveDataById != null && lootsSaveDataById.ContainsKey(elementId)) {
				lootSaveData = lootsSaveDataById[elementId];
				isCollected = lootSaveData.getIsCollected();
			}

			Loot loot = new Loot(nodeElementLoot, isCollected);
			loots[i] = loot;

			// create loot only if the loot was not collected
			if(!isCollected) {
				lootCreator.createNewGameObject(nodeElementLoot, loot);
			}

		}


		// load NPCs
		NpcCreator npcCreator = new NpcCreator();

		Dictionary<string, NpcSaveData> npcsSaveDataById = GameSaver.Instance.getNpcsSaveData();

		int npcsCount = currentNodeLevel.getNpcCount();

		for(int i=0 ; i<npcsCount ; i++) {
			
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

		ISpawnable currentSpawnable;

		if(mustSpawnPlayerAtHub) {
			//the player has died, the current node is the player
			currentSpawnable = hub;

		} else { 

			bool hasLoadedPlayer = false;

			//load saved player
			PlayerSaveData playerSaveData = GameSaver.Instance.getPlayerSaveData();
			if(playerSaveData != null) {
				
				hasLoadedPlayer = true;
				playerSaveData.assign(GameHelper.Instance.getPlayer());
			}

			if(lastSpawnable != null) {
				//something has triggered the load level (can be a link or a spawn)
				currentSpawnable = lastSpawnable;

			} else { 

				if(hasLoadedPlayer) {
					//done
					return;
				}

				if(currentNodeLevel == null) {
					//no level file provided : replace by default spawn element
					currentSpawnable = new Spawn();

				} else {
					
					//get spawn element of the level
					NodeElementSpawn nodeElementSpawn = currentNodeLevel.spawnElement;

					if(nodeElementSpawn == null) {
						//replace by default spawn element
						currentSpawnable = new Spawn();

					} else {
						
						spawn = new Spawn(nodeElementSpawn);
						currentSpawnable = spawn; 
					}
				}
			}
		}

		processPlayerLoad(currentSpawnable);

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

	public void processPlayerLoad(ISpawnable spawnable) {
		//move player in level with SPAWN / HUB / LINK / load from save

		if(spawnable == null) {
			throw new System.ArgumentException();
		}

		int posX = spawnable.getNextPosX();
		int posY = spawnable.getNextPosY();
		Direction direction = spawnable.getNextDirection();

		Player player = GameHelper.Instance.getPlayer();
		player.setMapPosition(posX, posY);
		player.setDirection(direction);

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

		string requiredLevelName = link.nextMap;

		if(string.IsNullOrEmpty(requiredLevelName)) {
			throw new System.InvalidOperationException("The requiredLevelName is empty");
		}

		lastSpawnable = link;

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

			GameManager.Instance.saveGame(false);

			Debug.Log("[GAME SAVED " + DateTime.Now + "]");
		}
		
	}

}
