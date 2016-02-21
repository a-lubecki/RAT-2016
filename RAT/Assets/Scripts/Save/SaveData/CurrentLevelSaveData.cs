using System;

[Serializable]
public class CurrentLevelSaveData {
	
	private string currentLevelName;
	
	public CurrentLevelSaveData(LevelManager levelManager) {
		
		if(levelManager == null) {
			throw new System.ArgumentException();
		}
		
		currentLevelName = levelManager.getCurrentLevelName();
	}
	
	public void assign(LevelManager levelManager) {
		
		if(levelManager == null) {
			throw new System.ArgumentException();
		}
		
		levelManager.loadNextLevel(currentLevelName);
	}
	
}


