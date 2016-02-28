using System;
using System.Collections.Generic;

[Serializable]
public class DoorListSaveData {

	private Dictionary<string, DoorSaveData> doorsDataById = new Dictionary<string, DoorSaveData>(); 

	public DoorListSaveData(Door[] doors) {
		
		if(doors == null) {
			return;
		}

		foreach(Door door in doors) {
			DoorSaveData doorData = new DoorSaveData(door);
			doorsDataById.Add(doorData.getId(), doorData);
		}

	}

	public Dictionary<string, DoorSaveData> getDoorsDataById() {
		return new Dictionary<string, DoorSaveData>(doorsDataById);
	}

}
