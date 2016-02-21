using System;

[Serializable]
public class PlayerStatsSaveData {
	
	public int skillPointHealth { get; private set; }
	public int skillPointEnergy { get; private set; }
	
	public string levelNameForlastHub { get; private set; }

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

