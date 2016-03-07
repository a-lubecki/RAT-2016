using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveDataV1 {

	public CurrentLevelSaveData currentLevelSaveData;
	public PlayerStatsSaveData playerStatsSaveData;
	public PlayerSaveData playerSaveData;
	public ItemInGridListSaveData itemsInGridSaveData;

	protected Dictionary<string, LevelSaveData> gameLevelSaveDataDictionary = new Dictionary<string, LevelSaveData>();

	public LevelSaveData getGameLevelSaveData(string levelName) {
		
		if(levelName == null || levelName.Length <= 0) {
			throw new System.ArgumentException();
		}

		LevelSaveData data;

		if(gameLevelSaveDataDictionary.ContainsKey(levelName)) {

			data = gameLevelSaveDataDictionary[levelName];

		} else {
			
			//create a new one
			data = new LevelSaveData();
			gameLevelSaveDataDictionary[levelName] = data;
		}

		return data;
	}

	public int getGameLevelSaveDataCount() {
		return gameLevelSaveDataDictionary.Count;
	}

}


[Serializable]
public class LevelSaveData {
	
	public HubSaveData hubSaveData;
	public DoorListSaveData doorListSaveData;
	public LootListSaveData lootListSaveData;
	public NpcListSaveData npcListSaveData;
	public ListenerEventListSaveData listenerEventListSaveData;

}
