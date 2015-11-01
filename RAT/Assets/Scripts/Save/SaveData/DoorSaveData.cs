using System;

[Serializable]
class DoorSaveData {
	
	public string id {get; private set; }
	private bool isOpened; 
	
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
