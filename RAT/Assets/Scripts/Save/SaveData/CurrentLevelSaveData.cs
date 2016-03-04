using System;

[Serializable]
public class CurrentLevelSaveData {
	
	protected string currentLevelName;

	public string getCurrentLevelName() {
		return currentLevelName;
	}
	
	public CurrentLevelSaveData(string currentLevelName) {
		
		if(string.IsNullOrEmpty(currentLevelName)) {
			throw new System.ArgumentException();
		}
		
		this.currentLevelName = currentLevelName;
	}

}


