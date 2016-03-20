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

				Debug.Log("Encryption : " + SystemInfo.deviceUniqueIdentifier + "\n" + BitConverter.ToString(ENCRYPTION_KEY) + "\n" + BitConverter.ToString(ENCRYPTION_IV));

				instance.loadAllFromFile();
			}
			return instance;
		}
	}


	public static readonly int CURRENT_VERSION = 1;
	public static readonly bool HAS_ENCRYPTION = false;

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
		Stream s = null;

		bool hasProblemOnSaveFile = false;
		try {
			if(HAS_ENCRYPTION) {
				s = new CryptoStream(fs, cryptoProvider.CreateDecryptor(ENCRYPTION_KEY, ENCRYPTION_IV), CryptoStreamMode.Read);
			} else {
				s = fs;
			}

			unserializeGame(bf, s);
			
		} catch(Exception e) {
			
			Debug.LogWarning(e);
			hasProblemOnSaveFile = true;

		} finally {

			if(s != null) {
				s.Close();
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

		if(Debug.isDebugBuild) {

			//copy the save before saving
			if(File.Exists(filePath)) {
				File.Copy(filePath, filePath + "_bak", true);
			}
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);

		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		Stream s = null;
		try {
			if(HAS_ENCRYPTION) {
				s = new CryptoStream(fs, cryptoProvider.CreateEncryptor(ENCRYPTION_KEY, ENCRYPTION_IV), CryptoStreamMode.Write);
			} else {
				s = fs;
			}

			serializeGame(bf, s);
			
		} catch(Exception e) {
			
			Debug.LogWarning(e);
			
		} finally {
			
			if(s != null) {
				s.Close();
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

	
	private LevelSaveData getCurrentGameLevelSaveData() {
		return gameSaveData.getGameLevelSaveData(GameManager.Instance.getCurrentLevelName());
	}


	public string getCurrentLevelName() {
		
		if(gameSaveData.currentLevelSaveData == null) {
			return null;
		}

		return gameSaveData.currentLevelSaveData.getCurrentLevelName();
	}
	
	public void saveCurrentLevel() {

		gameSaveData.currentLevelSaveData = new CurrentLevelSaveData(GameManager.Instance.getCurrentLevelName());
	}

	
	public PlayerStatsSaveData getPlayerStatsSaveData() {
		
		return gameSaveData.playerStatsSaveData;
	}

	public void savePlayerStats() {

		gameSaveData.playerStatsSaveData = new PlayerStatsSaveData(GameHelper.Instance.getPlayer());
	}
	
	public PlayerSaveData getPlayerSaveData() {
		
		return gameSaveData.playerSaveData;
	}

	public void savePlayer() {
				
		gameSaveData.playerSaveData = new PlayerSaveData(GameHelper.Instance.getPlayer());
	}
	
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
	
	public Dictionary<string, LootSaveData> getLootsSaveData() {

		LootListSaveData data = getCurrentGameLevelSaveData().lootListSaveData;
		if(data == null) {
			return null;
		}

		return data.getLootsDataById();
	}

	public void saveLoots() {
		
		Loot[] loots = GameHelper.Instance.getLoots();
		if(loots != null && loots.Length <= 0) {
			return;
		}
		
		getCurrentGameLevelSaveData().lootListSaveData = new LootListSaveData(loots);
	}

	public List<ItemInGridSaveData> getItemsInGridSaveData() {
		
		ItemInGridListSaveData data = gameSaveData.itemsInGridSaveData;
		if(data == null) {
			return null;
		}

		return data.getItemsInGrid();
	}

	public void saveInventory() {
		
		Inventory inventory = GameManager.Instance.getInventory();

		gameSaveData.itemsInGridSaveData = new ItemInGridListSaveData(inventory.getItems());
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

}

