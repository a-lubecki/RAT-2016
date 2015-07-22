using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SaverDoorsV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "doors";
	}
	
	public override bool isLevelSpecific() {
		return true;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}


	private DoorListData unserializedDoorsData;

	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		unserializedDoorsData = (DoorListData) bf.Deserialize(f);
	}

	protected override void assignUnserializedElement() {

		unserializedDoorsData.assign(GameHelper.Instance.getDoors());
	}


	protected override bool serializeElement(BinaryFormatter bf, FileStream f) {
		
		Door[] doors = GameHelper.Instance.getDoors();
		if(doors.Length <= 0) {
			return false;//no doors to serialize
		}

		DoorListData doorsData = new DoorListData(doors);

		bf.Serialize(f, doorsData);

		return true;
	}

}

[Serializable]
class DoorListData {

	private List<DoorData> doorsData = new List<DoorData>(); 

	public DoorListData(Door[] doors) {

		foreach(Door door in doors) {
			doorsData.Add(new DoorData(door));
		}

	}
	
	public void assign(Door[] doors) {

		Dictionary<string, Door> doorsById = new Dictionary<string, Door>(doors.Length);
		foreach(Door door in doors) {
			doorsById.Add(door.nodeElementDoor.nodeId.value, door);
		}

		foreach(DoorData doorData in doorsData) {
			Door door = doorsById[doorData.id];
			doorData.assign(door);
		}

	}

}

[Serializable]
class DoorData {
	
	public string id {get; private set; }
	private bool isOpened; 
	
	public DoorData(Door door) {
		id = door.nodeElementDoor.nodeId.value;
		isOpened = door.isOpened;
	}
	
	public void assign(Door door) {
		door.init(isOpened);
	}
}
