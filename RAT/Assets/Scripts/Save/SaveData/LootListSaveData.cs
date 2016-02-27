using System;
using System.Collections.Generic;

[Serializable]
public class LootListSaveData {

	private List<LootSaveData> lootsData = new List<LootSaveData>(); 

	public LootListSaveData(Loot[] loots) {
		
		if(loots == null) {
			return;
		}

		if(loots.Length <= 0) {
			lootsData.Clear();
			return;
		}

		foreach(Loot loot in loots) {
			lootsData.Add(new LootSaveData(loot));
		}

	}

	public Dictionary<string, LootSaveData> getLootsDataById() {

		Dictionary<string, LootSaveData> lootsById = new Dictionary<string, LootSaveData>(lootsData.Count);

		foreach(LootSaveData lootData in lootsData) {
			lootsById.Add(lootData.getId(), lootData);
		}

		return lootsById;
	}

}

