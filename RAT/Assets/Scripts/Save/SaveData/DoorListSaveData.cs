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
		}

		foreach(Door door in doors) {
			doorsData.Add(new DoorSaveData(door));
		}

	}
	
	public void assign(Door[] doors) {

		if(doors == null || doors.Length <= 0) {
			return;
		}

		Dictionary<string, Door> doorsById = new Dictionary<string, Door>(doors.Length);
		foreach(Door door in doors) {
			doorsById.Add(door.nodeElementDoor.nodeId.value, door);
		}

		foreach(DoorSaveData doorData in doorsData) {
			Door door = doorsById[doorData.id];
			if(door == null) {
				continue;
			}
			doorData.assign(door);
		}

	}
	
	public Dictionary<string, DoorSaveData> getDoorsDataById() {
		
		Dictionary<string, DoorSaveData> doorsById = new Dictionary<string, DoorSaveData>(doorsData.Count);
		
		foreach(DoorSaveData doorData in doorsData) {
			doorsById.Add(doorData.id, doorData);
		}
		
		return doorsById;
	}

}
