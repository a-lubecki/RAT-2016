using System;
using System.Collections.Generic;

[Serializable]
public class LootListSaveData {

	private Dictionary<string, LootSaveData> lootsDataById = new Dictionary<string, LootSaveData>(); 

	public LootListSaveData(Loot[] loots) {
		
		if(loots == null) {
			return;
		}

		foreach(Loot loot in loots) {
			LootSaveData lootData = new LootSaveData(loot);
			lootsDataById.Add(lootData.getId(), lootData);
		}

	}

	public Dictionary<string, LootSaveData> getLootsDataById() {
		return new Dictionary<string, LootSaveData>(lootsDataById);
	}

}

