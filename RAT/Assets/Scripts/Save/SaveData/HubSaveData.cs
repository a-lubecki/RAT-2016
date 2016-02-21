using System;

[Serializable]
public class HubSaveData {
	
	public bool isActivated { get; private set; } 
	
	public HubSaveData(Hub hub) {

		if(hub == null) {
			throw new System.ArgumentException();
		}

		isActivated = hub.isActivated;
	}
	
	public void assign(Hub hub) {
		
		if(hub == null) {
			throw new System.ArgumentException();
		}

		hub.init(isActivated);
	}
	
}
