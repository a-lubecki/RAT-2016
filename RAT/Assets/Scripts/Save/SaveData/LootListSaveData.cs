using System;
using System.Collections.Generic;

[Serializable]
class LootListSaveData {

	private List<LootSaveData> lootsData = new List<LootSaveData>(); 

	public LootListSaveData(Loot[] loots) {
		
		if(loots == null) {
			return;
		}

		if(loots.Length <= 0) {
			lootsData.Clear();
		}

		foreach(Loot loot in loots) {
			lootsData.Add(new LootSaveData(loot));
		}

	}
	
	public void assign(Loot[] loots) {

		if(loots == null || loots.Length <= 0) {
			return;
		}

		Dictionary<string, Loot> lootsById = new Dictionary<string, Loot>(loots.Length);
		foreach(Loot loot in loots) {
			lootsById.Add(loot.nodeElementLoot.nodeId.value, loot);
		}

		foreach(LootSaveData lootData in lootsData) {
			Loot loot = lootsById[lootData.id];
			if(loot == null) {
				continue;
			}

			lootData.assign(loot);
		}

	}

}
