using System;

[Serializable]
class HubSaveData {
	
	private bool isActivated; 
	
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
