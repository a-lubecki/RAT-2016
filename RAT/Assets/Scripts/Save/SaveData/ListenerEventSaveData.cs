using System;

[Serializable]
class ListenerEventSaveData {
	
	private string id;
	private bool isAchieved; 
	
	public ListenerEventSaveData(string id, IMapListener listener) {
		
		if(listener == null) {
			throw new System.ArgumentException();
		}
		
		this.id = id;
		isAchieved = listener.isEventAchieved(id);
	}
	
	public void assign(IMapListener listener) {
		
		if(listener == null) {
			throw new System.ArgumentException();
		}
		
		if(isAchieved) {
			listener.achieveEvent(id);
		}
	}
	
}
