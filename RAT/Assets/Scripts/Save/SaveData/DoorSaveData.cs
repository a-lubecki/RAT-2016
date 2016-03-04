using System;

[Serializable]
public class DoorSaveData {
	
	protected string id;
	protected bool isOpened;
	
	public string getId() {
		return id;
	}
	public bool getIsOpened() {
		return isOpened;
	}

	public DoorSaveData(Door door) {

		if(door == null) {
			throw new System.ArgumentException();
		}

		id = door.nodeElementDoor.nodeId.value;
		isOpened = door.isOpened;
	}
	
	public void assign(Door door) {

		if(door == null) {
			throw new System.ArgumentException();
		}

		door.init(isOpened);
	}
}
