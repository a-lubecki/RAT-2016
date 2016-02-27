using System;

[Serializable]
public class CurrentLevelSaveData {
	
	private string currentLevelName;

	public string getCurrentLevelName() {
		return currentLevelName;
	}
	
	public CurrentLevelSaveData(LevelManager levelManager) {
		
		if(levelManager == null) {
			throw new System.ArgumentException();
		}
		
		currentLevelName = levelManager.getCurrentLevelName();
	}

}


