using System;
using System.Collections.Generic;

[Serializable]
public class ListenerEventListSaveData {
	
	private List<ListenerEventSaveData> eventsData = new List<ListenerEventSaveData>(); 
	
	public ListenerEventListSaveData(IMapListener listener) {
		
		if(listener == null) {
			throw new System.ArgumentException();
		}

		foreach(string id in listener.getEventIds()) {
			eventsData.Add(new ListenerEventSaveData(id, listener));
		}
		
	}

	public List<ListenerEventSaveData> getListenersEventSaveData() {
		return new List<ListenerEventSaveData>(eventsData);
	}

}
