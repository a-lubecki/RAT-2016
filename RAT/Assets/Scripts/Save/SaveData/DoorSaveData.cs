using System;

[Serializable]
public class DoorSaveData {
	
	public string id { get; private set; }
	public bool isOpened { get; private set; } 
	
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
