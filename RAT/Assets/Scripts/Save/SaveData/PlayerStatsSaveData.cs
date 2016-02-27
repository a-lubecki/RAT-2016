using System;

[Serializable]
public class PlayerStatsSaveData {
	
	private int skillPointHealth;
	private int skillPointEnergy;
	
	private string levelNameForlastHub;

	public int getSkillPointHealth() {
		return skillPointHealth;
	}
	public int getSkillPointEnergy() {
		return skillPointEnergy;
	}
	public string getLevelNameForlastHub() {
		return levelNameForlastHub;
	}

	public PlayerStatsSaveData(Player player) {

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

