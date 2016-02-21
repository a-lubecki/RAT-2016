using System;

[Serializable]
public class LootSaveData {
	
	public string id { get; private set; }
	public bool isCollected { get; private set; }
	
	public LootSaveData(Loot loot) {

		if(loot == null) {
			throw new System.ArgumentException();
		}

		id = loot.nodeElementLoot.nodeId.value;
		isCollected = loot.isCollected;
	}
	
	public void assign(Loot loot) {

		if(loot == null) {
			throw new System.ArgumentException();
		}

		loot.init(isCollected);
	}
}
