using System;

[Serializable]
public class HubSaveData {
	
	protected bool isActivated;

	public bool getIsActivated() {
		return isActivated;
	}
	
	public HubSaveData(Hub hub) {

		if(hub == null) {
			throw new System.ArgumentException();
		}

		isActivated = hub.isActivated;
	}

}
