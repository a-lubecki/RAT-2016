using System;

[Serializable]
public class CurrentLevelSaveData {
	
	public string currentLevelName { get; private set; }
	
	public CurrentLevelSaveData(LevelManager levelManager) {
		
		if(levelManager == null) {
			throw new System.ArgumentException();
		}
		
		currentLevelName = levelManager.getCurrentLevelName();
	}

}


