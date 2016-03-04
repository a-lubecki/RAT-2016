using System;

[Serializable]
public class LootSaveData {
	
	protected string id;
	protected bool isCollected;

	public string getId() {
		return id;
	}
	public bool getIsCollected() {
		return isCollected;
	}

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
