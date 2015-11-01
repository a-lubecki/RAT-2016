using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Security.Cryptography;

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

	private static byte[] encryptionKey = ASCIIEncoding.ASCII.GetBytes ("45f8" + SystemInfo.deviceUniqueIdentifier.Substring (0, 8));

	public void loadAllFromFile() {
		
		Debug.Log("[LOAD begin : " + DateTime.Now + "]");

		string filePath = getFilePath();

		bool exists = File.Exists(filePath);

		if(!exists) {
			gameSaveData = new GameSaveDataV1();
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.OpenRead(filePath);

		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		CryptoStream cs = new CryptoStream(fs, cryptoProvider.CreateDecryptor(encryptionKey, encryptionKey), CryptoStreamMode.Read);

		unserializeGame(bf, cs);
		
		cs.Close();
		fs.Close();

		Debug.Log("[LOAD end : " + DateTime.Now + "]");
	}

	public void saveAllToFile() {
		
		Debug.Log("[SAVE begin : " + DateTime.Now + "]");

		string filePath = getFilePath();
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);

		DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
		CryptoStream cs = new CryptoStream(fs, cryptoProvider.CreateEncryptor(encryptionKey, encryptionKey), CryptoStreamMode.Write);

		serializeGame(bf, cs);

		cs.Close();
		fs.Close();
		
		Debug.Log("[SAVE end : " + DateTime.Now + "]");
	}

	private void serializeGame(BinaryFormatter bf, Stream stream) {

		try {
			bf.Serialize(stream, CURRENT_VERSION);
			bf.Serialize(stream, gameSaveData);

		} catch(Exception e) {
			
			Debug.LogException(e);

		} finally {
			stream.Close();
		}

	}

	private void unserializeGame(BinaryFormatter bf, Stream stream) {
		
		try {
			int unserializedVersion = (int) bf.Deserialize(stream);

			if(unserializedVersion != CURRENT_VERSION) {
				throw new Exception("The version of the game save is incorrect : current = " + CURRENT_VERSION + ", unserialized = " + unserializedVersion);
			}

			gameSaveData = (GameSaveDataV1) bf.Deserialize(stream);
		
		} catch(Exception e) {

			gameSaveData = new GameSaveDataV1();
			
			Debug.LogException(e);
			
		} finally {
			stream.Close();
		}

	}

	
	private GameLevelSaveData getCurrentGameLevelSaveData() {
		return gameSaveData.getGameLevelSaveData(GameHelper.Instance.getLevelManager().getCurrentLevelName());
	}

	
	public void saveCurrentLevel() {

		gameSaveData.currentLevelSaveData = new CurrentLevelSaveData(GameHelper.Instance.getLevelManager());
	}
	
	public bool loadCurrentLevel() {
	
		if(gameSaveData.currentLevelSaveData == null) {
			return false;
		}
		
		gameSaveData.currentLevelSaveData.assign(GameHelper.Instance.getLevelManager());

		return true;
	}
	
	public void savePlayerStats() {

		gameSaveData.playerStatsSaveData = new PlayerStatsSaveData(GameHelper.Instance.getPlayer());
	}
	
	public bool loadPlayerStats() {
		
		if(gameSaveData.playerStatsSaveData == null) {
			return false;
		}
		
		gameSaveData.playerStatsSaveData.assign(GameHelper.Instance.getPlayer());

		return true;
	}
	
	public void savePlayer() {
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		gameSaveData.playerSaveData = new PlayerSaveData(
			playerGameObject.GetComponent<Player>(),
			playerGameObject.GetComponent<PlayerControls>());
	}
	
	public bool loadPlayer() {

		if(gameSaveData.playerSaveData == null) {
			return false;
		}
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		gameSaveData.playerSaveData.assign(
			playerGameObject.GetComponent<Player>(),
			playerGameObject.GetComponent<PlayerControls>());

		return true;
	}
	
	public void deletePlayer() {

		if(gameSaveData.playerSaveData == null) {
			return;
		}

		gameSaveData.playerSaveData = null;
	}

	public void saveHub() {
		
		Hub hub = GameHelper.Instance.getHub();
		if(hub == null) {
			return;
		}
		
		getCurrentGameLevelSaveData().hubSaveData = new HubSaveData(hub);
	}
	
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
	}
	
	public void saveDoors() {
		
		Door[] doors = GameHelper.Instance.getDoors();
		if(doors.Length <= 0) {
			return;
		}
		
		getCurrentGameLevelSaveData().doorListSaveData = new DoorListSaveData(doors);
	}
	
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
	}
	
	public void saveNpcs() {
		
		Npc[] npcs = GameHelper.Instance.getNpcs();
		if(npcs.Length <= 0) {
			return;
		}
		
		getCurrentGameLevelSaveData().npcListSaveData = new NpcListSaveData(npcs);
	}
	
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
	}
	
	public void deleteNpcs() {
		
		getCurrentGameLevelSaveData().npcListSaveData = null;
	}
	
	public void saveListenerEvents() {
				
		IMapListener mapListener = GameHelper.Instance.getCurrentMapListener();
		if(mapListener == null) {
			return;
		}
		
		getCurrentGameLevelSaveData().listenerEventListSaveData = new ListenerEventListSaveData(mapListener);
	}
	
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
	}

}

