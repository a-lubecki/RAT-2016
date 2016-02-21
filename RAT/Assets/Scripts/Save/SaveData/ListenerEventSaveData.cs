using System;

[Serializable]
public class ListenerEventSaveData {
	
	public string id { get; private set; }
	public bool isAchieved { get; private set; }
	
	public ListenerEventSaveData(string id, IMapListener listener) {
		
		if(listener == null) {
			throw new System.ArgumentException();
		}
		
		this.id = id;
		isAchieved = listener.isEventAchieved(id);
	}
	
}
