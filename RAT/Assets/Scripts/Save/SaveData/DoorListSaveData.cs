using System;
using System.Collections.Generic;

[Serializable]
public class DoorListSaveData {

	private List<DoorSaveData> doorsData = new List<DoorSaveData>(); 

	public DoorListSaveData(Door[] doors) {
		
		if(doors == null) {
			return;
		}

		if(doors.Length <= 0) {
			doorsData.Clear();
			return;
		}

		foreach(Door door in doors) {
			doorsData.Add(new DoorSaveData(door));
		}

	}

	public Dictionary<string, DoorSaveData> getDoorsDataById() {
		
		Dictionary<string, DoorSaveData> doorsById = new Dictionary<string, DoorSaveData>(doorsData.Count);
		
		foreach(DoorSaveData doorData in doorsData) {
			doorsById.Add(doorData.getId(), doorData);
		}
		
		return doorsById;
	}

}
