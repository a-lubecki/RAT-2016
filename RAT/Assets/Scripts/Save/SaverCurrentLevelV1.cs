using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaverCurrentLevelV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "level";
	}
	
	public override bool isLevelSpecific() {
		return false;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}

	
	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		CurrentLevelData currentLevelData = (CurrentLevelData) bf.Deserialize(f);

		currentLevelData.assign(GameHelper.Instance.getLevelManager());

	}
	
	protected override void serializeElement(BinaryFormatter bf, FileStream f) {

		CurrentLevelData currentLevelData = new CurrentLevelData(
			GameHelper.Instance.getLevelManager());

		bf.Serialize(f, currentLevelData);

	}

}

[Serializable]
class CurrentLevelData {
	
	private string currentLevelName;

	public CurrentLevelData(LevelManager levelManager) {

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

