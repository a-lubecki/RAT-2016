using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Level;
using System;
using SmartLocalization;


public class LevelManager : MonoBehaviour {

	private static string nextLevelName;//used to keep the information between levels
	private static BaseNodeElement lastNodeElementTrigger;//last trigger, spawn, link => used to keep the information between levels
	private static bool mustSpawnPlayerAtHub = false;

	private static string currentLevelName;
	private static NodeLevel currentNodeLevel;//optional

	private bool isRunningSaverLoop = false;
	private Coroutine coroutineSaveLoop;
	
	public string chosenLocalization = "en-US";


	void Start () {

		//set the language
		List<SmartCultureInfo> supportedLanguages = LanguageManager.Instance.GetSupportedLanguages();
		
		SmartCultureInfo chosenSmartCultureInfo = supportedLanguages[0];
		foreach (SmartCultureInfo info in supportedLanguages) {
			if(chosenLocalization.Equals(info.languageCode)) {
				chosenSmartCultureInfo = info;
				break;
			}
		}
		
		LanguageManager.Instance.ChangeLanguage(chosenSmartCultureInfo);


		if(currentLevelName == null) {

			//load a level
			if(!GameSaver.Instance.loadCurrentLevel()) {

				//if no saved data, load the very first level				
				loadNextLevel(Constants.FIRST_LEVEL_NAME);
			}

		}

	}


	void OnLevelWasLoaded(int level) {

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

			GameHelper.Instance.getPlayerControls().disableControls();
			
			GameSaver.Instance.savePlayer();
			GameSaver.Instance.saveNpcs();
		}

		//no fadein if level is the first
		AutoFade.LoadLevel(0, hasCurrentLevel ? 0.3f : 0, 0.3f, Color.black);
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


		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		PlayerControls playerControls = playerGameObject.GetComponent<PlayerControls>();
		playerControls.setInitialPosition(nodePosition, nodeDirection);

	}

	private void initPlayerStats() {
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();

		//load player stats
		if(!GameSaver.Instance.loadPlayerStats()) {
			//very first init of the player stats
			player.initStats(5, 5);
		
		} else if(mustSpawnPlayerAtHub) {

			player.reinitLifeAndStamina();
		}

	}


	public void processPlayerRespawn() {
		
		if(isAboutToLoadNextLevel()) {
			//already processing for next level
			return;
		}

		//load next level with HUB

		string requiredLevelName = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>().levelNameForLastHub;

		if(string.IsNullOrEmpty(requiredLevelName)) {
			throw new System.InvalidOperationException("levelNameForlastHub is empty");
		}

		mustSpawnPlayerAtHub = true;

		//these data are now obsolete
		GameSaver.Instance.deletePlayer();
		GameSaver.Instance.deleteNpcs();

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
			
			Debug.Log("[GAME SAVED " + DateTime.Now + "]");
		}
		
	}

}
