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

	public Player player { get; private set; }
	public Spawn spawn { get; private set; }
	public Link[] links { get; private set; }
	public Door[] doors { get; private set; }
	public Hub hub { get; private set; }
	public Loot[] loots { get; private set; }
	public Note[] notes { get; private set; }
	public Npc[] npcs { get; private set; }


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

	void Stop () {
		
		enablePlayerGameObjects(false);

	}

	private void enablePlayerGameObjects(bool enabled) {
		
		Player player = GameHelper.Instance.getPlayer();
		if (player == null) {
			return;
		}

		PlayerBehavior playerBehavior = player.findBehavior<PlayerBehavior>();//TODO refaire

		if(playerBehavior != null) {
			playerBehavior.enabled = enabled;
		}

		PlayerRendererBehavior playerRendererBehavior = player.findBehavior<PlayerRendererBehavior>();//TODO refaire

		if(playerRendererBehavior != null) {
			playerRendererBehavior.enabled = enabled;
		}
	}

	void OnLevelWasLoaded(int level) {

		enablePlayerGameObjects(false);

		if(SceneManager.GetActiveScene().buildIndex != (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL)) {
			return;
		}

		if(!isAboutToLoadNextLevel()) {
			return;
		}

		if(isVeryFirstStart) {
			MessageDisplayer.Instance.displayMessages(new Message(this, "Nouvel objectif : Sortir du complexe"));//TODO test
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


		// load notes
		NoteCreator noteCreator = new NoteCreator();

		int notesCount = currentNodeLevel.getNoteCount();
		notes = new Note[notesCount];

		for(int i=0 ; i<notesCount ; i++) {

			NodeElementNote nodeElementNote = currentNodeLevel.getNote(i);

			Note note = new Note(nodeElementNote);
			notes[i] = note;

			noteCreator.createNewGameObject(nodeElementNote, note);

		}


		// load NPCs
		NpcCreator npcCreator = new NpcCreator();

		Dictionary<string, NpcSaveData> npcsSaveDataById = GameSaver.Instance.getNpcsSaveData();

		int npcsCount = currentNodeLevel.getNpcCount();
		npcs = new Npc[npcsCount];

		for(int i=0 ; i<npcsCount ; i++) {
			
			NodeElementNpc nodeElementNpc = currentNodeLevel.getNpc(i);
			string elementId = nodeElementNpc.nodeId.value;

			bool setCurrentPosition = false;
			int currentPosX = 0;
			int currentPosY = 0;
			int currentAngleDegrees;
			bool setCurrentLife = false;
			int currentLife = 0;
			NpcSaveData npcSaveData = null;
			
			//init if previously saved
			if(npcsSaveDataById != null && npcsSaveDataById.ContainsKey(elementId)) {
				
				npcSaveData = npcsSaveDataById[elementId];

				setCurrentLife = true;
				currentLife = npcSaveData.getCurrentLife();

				currentPosX = npcSaveData.getCurrentPosX();
				currentPosY = npcSaveData.getCurrentPosY();
				currentAngleDegrees = npcSaveData.getCurrentAngleDegrees();

			} else {
				
				Vector2 currentPos = GameHelper.newPositionOnMap(nodeElementNpc.nodePosition.x, nodeElementNpc.nodePosition.y);
				currentPosX = currentPos.x;
				currentPosY = currentPos.y;
				currentAngleDegrees = Character.directionToAngle(nodeElementNpc.nodeDirection.value);
			}

			Npc npc = new Npc(nodeElementNpc, setCurrentLife, currentLife, currentPosX, currentPosY, currentAngleDegrees);
			npcs[i] = npc;

			if(!setCurrentLife || currentLife > 0) {//not dead

				npcCreator.createNewGameObject(nodeElementNpc, npc);

			} else {
				//create body
				
				//TODO TEST !!!
				//npcBodyCreator.createNewGameObject(nodeElementNpc, npc, currentPosX, currentPosY);
				//TODO TEST !!!
			}
		}

	}

	private void spawnPlayer() {
		
		string levelNameForLastHub = null;
		int skillPointsHealth;
		int skillPointsEnergy;
		bool setLife = false;
		int life = 0;
		bool setStamina = false;
		int stamina = 0;
		int xp = 0;
		float realPosX = 0;
		float realPosY = 0;
		int angleDegrees = 0;

		//load player stats
		PlayerStatsSaveData playerStatsSaveData = GameSaver.Instance.getPlayerStatsSaveData();
		if(playerStatsSaveData != null) {
			
			levelNameForLastHub = playerStatsSaveData.getLevelNameForlastHub();

			skillPointsHealth = playerStatsSaveData.getSkillPointsHealth();
			skillPointsEnergy = playerStatsSaveData.getSkillPointsEnergy();

		} else {
			
			skillPointsHealth = 10;
			skillPointsEnergy = 10;
		}

		bool hasLoadedPlayerFromSave = false;

		//load saved player
		PlayerSaveData playerSaveData = GameSaver.Instance.getPlayerSaveData();
		if(playerSaveData != null) {

			if(!mustSpawnPlayerAtHub) {

				hasLoadedPlayerFromSave = true;

				setLife = true;
				life = playerSaveData.getCurrentLife();
				setStamina = true;
				stamina = playerSaveData.getCurrentStamina();
				xp = playerSaveData.getCurrentXp();
				realPosX = playerSaveData.getCurrentPosX();
				realPosY = playerSaveData.getCurrentPosY();
				angleDegrees = playerSaveData.getCurrentAngleDegrees();
			}
		}


		NodeLevel currentNodeLevel = GameManager.Instance.getCurrentNodeLevel();

		ISpawnable currentSpawnable = null;

		if(mustSpawnPlayerAtHub) {
			//the player has died, the current node is the hub
			currentSpawnable = hub;

		} else { 

			if(lastSpawnable != null) {
				//something has triggered the load level (can be a link or a spawn)
				currentSpawnable = lastSpawnable;

			} else { 

				if(!hasLoadedPlayerFromSave) {

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
		}


		if(currentSpawnable != null) {
			realPosX = currentSpawnable.getNextPosX();
			realPosY = currentSpawnable.getNextPosY();
			angleDegrees = Character.directionToAngle(currentSpawnable.getNextDirection());
		}

		player = new Player(skillPointsHealth, skillPointsEnergy, setLife, life, setStamina, stamina, xp, realPosX, realPosY, angleDegrees);
		if(!string.IsNullOrEmpty(levelNameForLastHub)) {
			player.levelNameForLastHub = levelNameForLastHub;
		}

		Transform mapTransform = GameHelper.Instance.getMapGameObject().transform;
		PlayerBehavior playerBehavior = mapTransform.Find(Constants.GAME_OBJECT_NAME_PLAYER).GetComponent<PlayerBehavior>();
		PlayerRendererBehavior playerRendererBehavior = mapTransform.Find(Constants.GAME_OBJECT_NAME_PLAYER_RENDERER).GetComponent<PlayerRendererBehavior>();

		playerBehavior.init(player);
		playerRendererBehavior.init(player);

		enablePlayerGameObjects(true);

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

			GameHelper.Instance.getPlayer().disableControls(this);

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
		
		string requiredLevelName = player.levelNameForLastHub;
		
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
