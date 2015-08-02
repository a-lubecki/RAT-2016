using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaverPlayerStatsV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "playerStats";
	}
	
	public override bool isLevelSpecific() {
		return false;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}


	private PlayerStatsData unserializedPlayerStatsData;

	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		unserializedPlayerStatsData = (PlayerStatsData) bf.Deserialize(f);
	}
	
	protected override void assignUnserializedElement() {
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		unserializedPlayerStatsData.assign(
			playerGameObject.GetComponent<Player>());
	}


	protected override bool serializeElement(BinaryFormatter bf, FileStream f) {

		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();

		PlayerStatsData playerStatsData = new PlayerStatsData(
			playerGameObject.GetComponent<Player>());

		bf.Serialize(f, playerStatsData);
		
		return true;
	}

}

[Serializable]
class PlayerStatsData {
	
	private int skillPointHealth;
	private int skillPointEnergy;
	
	private string levelNameForlastHub;

	public PlayerStatsData(Player player) {

		if(player == null) {
			throw new System.ArgumentException();
		}
		
		skillPointHealth = player.skillPointHealth;
		skillPointEnergy = player.skillPointEnergy;
		
		levelNameForlastHub = player.levelNameForLastHub;
	}
	
	public void assign(Player player) {

		if(player == null) {
			throw new System.ArgumentException();
		}

		player.initStats(skillPointHealth, skillPointEnergy);

		player.levelNameForLastHub = levelNameForlastHub;
	}

}

