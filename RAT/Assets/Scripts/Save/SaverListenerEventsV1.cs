using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SaverListenerEventsV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "events";
	}
	
	public override bool isLevelSpecific() {
		return true;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}


	private ListenerEventListData unserializedEventsData;

	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		unserializedEventsData = (ListenerEventListData) bf.Deserialize(f);
	}

	protected override void assignUnserializedElement() {

		IMapListener listener = GameHelper.Instance.getCurrentMapListener();
		if(listener == null) {
			return;//no listener to serialize
		}

		unserializedEventsData.assign(listener);
	}


	protected override bool serializeElement(BinaryFormatter bf, FileStream f) {
		
		IMapListener listener = GameHelper.Instance.getCurrentMapListener();
		if(listener == null) {
			return false;//no listener to serialize
		}

		ListenerEventListData listenerData = new ListenerEventListData(listener);

		bf.Serialize(f, listenerData);

		return true;
	}

}

[Serializable]
class ListenerEventListData {
	
	private List<ListenerEventData> eventsData = new List<ListenerEventData>(); 
	
	public ListenerEventListData(IMapListener listener) {
		
		foreach(string id in listener.getEventIds()) {
			eventsData.Add(new ListenerEventData(id, listener));
		}
		
	}
	
	public void assign(IMapListener listener) {

		foreach(ListenerEventData eventData in eventsData) {
			eventData.assign(listener);
		}
		
	}
	
}

[Serializable]
class ListenerEventData {
	
	private string id;
	private bool isAchieved; 

	public ListenerEventData(string id, IMapListener listener) {
		this.id = id;
		isAchieved = listener.isEventAchieved(id);
	}
	
	public void assign(IMapListener listener) {

		if(isAchieved) {
			listener.achieveEvent(id);
		}
	}

}

