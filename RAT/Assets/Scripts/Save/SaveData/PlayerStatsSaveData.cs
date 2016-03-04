using System;

[Serializable]
public class PlayerStatsSaveData {
	
	protected int skillPointHealth;
	protected int skillPointEnergy;
	
	protected string levelNameForlastHub;

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

