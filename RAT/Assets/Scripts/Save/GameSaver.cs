using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;

public class GameSaver {
	
	private static GameSaver instance;
	
	private GameSaver() {}
	
	public static GameSaver Instance {
		
		get {
			if (instance == null) {
				instance = new GameSaver();
				instance.loadAllFromFile();
			}
			return instance;
		}
	}


	public static readonly int CURRENT_VERSION = 1;

	private GameSaveDataV1 gameSaveData = new GameSaveDataV1();

	
	private string getFilePath() {
		return Application.persistentDataPath + "/save_v" + CURRENT_VERSION;
	}
	
	private static readonly byte[] ENCRYPTION_KEY = ASCIIEncoding.ASCII.GetBytes("d78fmlz9");
	private static readonly byte[] ENCRYPTION_IV = ASCIIEncoding.ASCII.GetBytes("65f8" + SystemInfo.deviceUniqueIdentifier.Substring (0, 8));

	public void loadAllFromFile() {
		
		Debug.Log("[LOAD begin : " + DateTime.Now + "]");

		string filePath = getFilePath();

		bool exists = File.Exists(filePath);

		if(!exists) {
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.OpenRead(filePath);

		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		CryptoStream cs = null;

		bool hasProblemOnSaveFile = false;
		try {
			cs = new CryptoStream(fs, cryptoProvider.CreateDecryptor(ENCRYPTION_KEY, ENCRYPTION_IV), CryptoStreamMode.Read);

			unserializeGame(bf, cs);
			
		} catch(Exception e) {
			
			Debug.LogWarning(e);
			hasProblemOnSaveFile = true;

		} finally {

			if(cs != null) {
				cs.Close();
			}

			fs.Close();
		}

		if(hasProblemOnSaveFile) {	
			deleteSave();
		}

		Debug.Log("[LOAD end : " + DateTime.Now + "]");
	}

	public void saveAllToFile() {
		
		Debug.Log("[SAVE begin : " + DateTime.Now + "]");

		string filePath = getFilePath();
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);

		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		CryptoStream cs = null;
		try {
			cs = new CryptoStream(fs, cryptoProvider.CreateEncryptor(ENCRYPTION_KEY, ENCRYPTION_IV), CryptoStreamMode.Write);

			serializeGame(bf, cs);
			
		} catch(Exception e) {
			
			Debug.LogWarning(e);
			
		} finally {
			
			if(cs != null) {
				cs.Close();
			}
			
			fs.Close();
		}
		
		Debug.Log("[SAVE end : " + DateTime.Now + "]");
	}

	private void serializeGame(BinaryFormatter bf, Stream stream) {
		
		bf.Serialize(stream, CURRENT_VERSION);
		bf.Serialize(stream, gameSaveData);

	}

	private void unserializeGame(BinaryFormatter bf, Stream stream) {
		
		int unserializedVersion = (int) bf.Deserialize(stream);
		
		if(unserializedVersion != CURRENT_VERSION) {
			Debug.LogWarning("The version of the game save is incorrect : current = " + CURRENT_VERSION + ", unserialized = " + unserializedVersion);
			return;
		}
		
		gameSaveData = (GameSaveDataV1) bf.Deserialize(stream);

	}

	private void deleteSave() {

		string path = getFilePath();
		if(!File.Exists(path)) {
			return;
		}

		File.Delete(path);
	}

	
	private GameLevelSaveData getCurrentGameLevelSaveData() {
		return gameSaveData.getGameLevelSaveData(GameHelper.Instance.getLevelManager().getCurrentLevelName());
	}


	public string getCurrentLevelName() {
		
		if(gameSaveData.currentLevelSaveData == null) {
			return null;
		}

		return gameSaveData.currentLevelSaveData.currentLevelName;
	}
	
	public void saveCurrentLevel() {

		gameSaveData.currentLevelSaveData = new CurrentLevelSaveData(GameHelper.Instance.getLevelManager());
	}

	
	public PlayerStatsSaveData getPlayerStatsSaveData() {
		
		return gameSaveData.playerStatsSaveData;
	}

	public void savePlayerStats() {

		gameSaveData.playerStatsSaveData = new PlayerStatsSaveData(GameHelper.Instance.getPlayer());
	}
	/*
	public bool loadPlayerStats() {
		
		if(gameSaveData.playerStatsSaveData == null) {
			return false;
		}
		
		gameSaveData.playerStatsSaveData.assign(GameHelper.Instance.getPlayer());

		return true;
	}*/
	
	public PlayerSaveData getPlayerSaveData() {
		
		return gameSaveData.playerSaveData;
	}

	public void savePlayer() {
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		gameSaveData.playerSaveData = new PlayerSaveData(playerGameObject.GetComponent<Player>());
	}
	/*
	public bool loadPlayer() {

		if(gameSaveData.playerSaveData == null) {
			return false;
		}
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		gameSaveData.playerSaveData.assign(playerGameObject.GetComponent<Player>());

		return true;
	}*/
	
	public void deletePlayer() {

		if(gameSaveData.playerSaveData == null) {
			return;
		}

		gameSaveData.playerSaveData = null;
	}
	
	
	public HubSaveData getHubSaveData() {
		return getCurrentGameLevelSaveData().hubSaveData;
	}

	public void saveHub() {
		
		Hub hub = GameHelper.Instance.getHub();
		if(hub == null) {
			return;
		}
		
		getCurrentGameLevelSaveData().hubSaveData = new HubSaveData(hub);
	}
	/*
	public bool loadHub() {
		
		Hub hub = GameHelper.Instance.getHub();
		if(hub == null) {
			return true;
		}
		
		GameLevelSaveData data = getCurrentGameLevelSaveData();
		
		if(data.hubSaveData == null) {
			return false;
		}
		
		data.hubSaveData.assign(hub);
		
		return true;
	}*/
	
	public Dictionary<string, DoorSaveData> getDoorsSaveData() {

		DoorListSaveData data = getCurrentGameLevelSaveData().doorListSaveData;
		if(data == null) {
			return null;
		}

		return data.getDoorsDataById();
	}

	public void saveDoors() {
		
		Door[] doors = GameHelper.Instance.getDoors();
		if(doors.Length <= 0) {
			return;
		}
		
		getCurrentGameLevelSaveData().doorListSaveData = new DoorListSaveData(doors);
	}
	/*
	public bool loadDoors() {
		
		Door[] doors = GameHelper.Instance.getDoors();
		if(doors.Length <= 0) {
			return true;
		}
		
		GameLevelSaveData data = getCurrentGameLevelSaveData();
		
		if(data.doorListSaveData == null) {
			return false;
		}
		
		data.doorListSaveData.assign(doors);
		
		return true;
	}*/
	
	public Dictionary<string, LootSaveData> getLootsSaveData() {

		LootListSaveData data = getCurrentGameLevelSaveData().lootListSaveData;
		if(data == null) {
			return null;
		}

		return data.getLootsDataById();
	}

	public void saveLoots() {
		
		Loot[] loots = GameHelper.Instance.getLoots();
		if(loots.Length <= 0) {
			return;
		}
		
		getCurrentGameLevelSaveData().lootListSaveData = new LootListSaveData(loots);
	}
	
	public Dictionary<string, NpcSaveData> getNpcsSaveData() {

		NpcListSaveData data = getCurrentGameLevelSaveData().npcListSaveData;
		if(data == null) {
			return null;
		}

		return data.getNpcsDataById();
	}
	
	public void saveNpcs() {
		
		Npc[] npcs = GameHelper.Instance.getNpcs();
		if(npcs.Length <= 0) {
			return;
		}
		
		getCurrentGameLevelSaveData().npcListSaveData = new NpcListSaveData(npcs);
	}
	/*
	public bool loadNpcs() {

		Npc[] npcs = GameHelper.Instance.getNpcs();
		if(npcs.Length <= 0) {
			return true;
		}
		
		GameLevelSaveData data = getCurrentGameLevelSaveData();
		
		if(data.npcListSaveData == null) {
			return false;
		}
		
		data.npcListSaveData.assign(npcs);
		
		return true;
	}*/
	
	public void deleteNpcs() {
		
		getCurrentGameLevelSaveData().npcListSaveData = null;
	}

	public List<ListenerEventSaveData> getListenerEvents() {
		
		ListenerEventListSaveData data = getCurrentGameLevelSaveData().listenerEventListSaveData;
		if(data == null) {
			return null;
		}

		return data.getListenersEventSaveData();
	}

	public void saveListenerEvents() {
				
		IMapListener mapListener = GameHelper.Instance.getCurrentMapListener();
		if(mapListener == null) {
			return;
		}
		
		getCurrentGameLevelSaveData().listenerEventListSaveData = new ListenerEventListSaveData(mapListener);
	}
	/*
	public bool loadListenerEvents() {
		
		IMapListener mapListener = GameHelper.Instance.getCurrentMapListener();
		if(mapListener == null) {
			return true;
		}
		
		GameLevelSaveData data = getCurrentGameLevelSaveData();
		
		if(data.listenerEventListSaveData == null) {
			return false;
		}
		
		data.listenerEventListSaveData.assign(mapListener);
		
		return true;
	}*/

}

