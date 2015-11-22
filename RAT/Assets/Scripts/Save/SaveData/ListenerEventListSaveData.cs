using System;
using System.Collections.Generic;

[Serializable]
class ListenerEventListSaveData {
	
	private List<ListenerEventSaveData> eventsData = new List<ListenerEventSaveData>(); 
	
	public ListenerEventListSaveData(IMapListener listener) {
		
		if(listener == null) {
			throw new System.ArgumentException();
		}

		foreach(string id in listener.getEventIds()) {
			eventsData.Add(new ListenerEventSaveData(id, listener));
		}
		
	}
	
	public void assign(IMapListener listener) {

		if(listener == null) {
			throw new System.ArgumentException();
		}

		foreach(ListenerEventSaveData eventData in eventsData) {
			eventData.assign(listener);
		}
		
	}
	
}