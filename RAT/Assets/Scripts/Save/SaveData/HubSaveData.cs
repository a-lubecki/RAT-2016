using System;

[Serializable]
public class HubSaveData {
	
	private bool isActivated;

	public bool getIsActivated() {
		return isActivated;
	}
	
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
