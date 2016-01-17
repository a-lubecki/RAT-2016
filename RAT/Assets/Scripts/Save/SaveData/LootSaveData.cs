using System;

[Serializable]
class LootSaveData {
	
	public string id { get; private set; }
	private bool isCollected;
	
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
