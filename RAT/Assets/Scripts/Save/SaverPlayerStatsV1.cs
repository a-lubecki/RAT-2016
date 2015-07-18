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

	
	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		PlayerStatsData playerStatsData = (PlayerStatsData) bf.Deserialize(f);
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();

		playerStatsData.assign(
			playerGameObject.GetComponent<Player>());
	}
	
	protected override void serializeElement(BinaryFormatter bf, FileStream f) {

		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();

		PlayerStatsData playerStatsData = new PlayerStatsData(
			playerGameObject.GetComponent<Player>());

		bf.Serialize(f, playerStatsData);

	}

}

[Serializable]
class PlayerStatsData {
	
	private int skillPointHealth;
	private int skillPointEnergy;

	private int currentLife;
	private int currentStamina;
	
	private string levelNameForlastHub;

	public PlayerStatsData(Player player) {

		if(player == null) {
			throw new System.ArgumentException();
		}
		
		skillPointHealth = player.skillPointHealth;
		skillPointEnergy = player.skillPointEnergy;

		currentLife = player.life;
		currentStamina = player.stamina;

		levelNameForlastHub = player.levelNameForlastHub;
	}
	
	public void assign(Player player) {

		if(player == null) {
			throw new System.ArgumentException();
		}

		player.init(skillPointHealth, skillPointEnergy, currentLife, currentStamina);

		player.levelNameForlastHub = levelNameForlastHub;
	}

}

