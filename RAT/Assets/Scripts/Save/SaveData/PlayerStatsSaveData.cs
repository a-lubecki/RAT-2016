using System;

[Serializable]
public class PlayerStatsSaveData {
	
	protected int skillPointsHealth;
	protected int skillPointsEnergy;
	
	protected string levelNameForlastHub;

	public int getSkillPointsHealth() {
		return skillPointsHealth;
	}
	public int getSkillPointsEnergy() {
		return skillPointsEnergy;
	}
	public string getLevelNameForlastHub() {
		return levelNameForlastHub;
	}

	public PlayerStatsSaveData(Player player) {

		if(player == null) {
			throw new System.ArgumentException();
		}

		skillPointsHealth = player.skillPointsHealth;
		skillPointsEnergy = player.skillPointsEnergy;
		
		levelNameForlastHub = player.levelNameForLastHub;
	}

}

