using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveDataV1 {

	public CurrentLevelSaveData currentLevelSaveData;
	public PlayerStatsSaveData playerStatsSaveData;
	public PlayerSaveData playerSaveData;

	private Dictionary<string, GameLevelSaveData> gameLevelSaveDataDictionary = new Dictionary<string, GameLevelSaveData>();

	public GameLevelSaveData getGameLevelSaveData(string levelName) {
		
		if(levelName == null || levelName.Length <= 0) {
			throw new System.ArgumentException();
		}

		GameLevelSaveData data;

		if(gameLevelSaveDataDictionary.ContainsKey(levelName)) {

			data = gameLevelSaveDataDictionary[levelName];

		} else {
			
			//create a new one
			data = new GameLevelSaveData();
			gameLevelSaveDataDictionary[levelName] = data;
		}

		return data;
	}

	public int getGameLevelSaveDataCount() {
		return gameLevelSaveDataDictionary.Count;
	}

}


[Serializable]
public class GameLevelSaveData {
	
	public HubSaveData hubSaveData;
	public DoorListSaveData doorListSaveData;
	public LootListSaveData lootListSaveData;
	public NpcListSaveData npcListSaveData;
	public ListenerEventListSaveData listenerEventListSaveData;

}
