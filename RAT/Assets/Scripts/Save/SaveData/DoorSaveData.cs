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

		id = door.id;
		isOpened = door.isOpened;
	}

}
