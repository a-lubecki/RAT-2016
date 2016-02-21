using System;

[Serializable]
public class PlayerSaveData {

	public int currentPosX { get; private set; }
	public int currentPosY { get; private set; }
	public int currentAngleDegrees { get; private set; }
	
	public int currentLife { get; private set; }
	public int currentStamina { get; private set; }
	public int currentXp { get; private set; }

	public PlayerSaveData(Player player) {
		
		if(player == null) {
			throw new System.ArgumentException();
		}

		currentPosX = (int)(player.transform.position.x);
		currentPosY = (int)(player.transform.position.y);
		currentAngleDegrees = (int)player.angleDegrees;
		
		currentLife = player.life;
		currentStamina = player.stamina;
		currentXp = player.xp;
	}
	
	public void assign(Player player) {
		
		if(player == null) {
			throw new System.ArgumentException();
		}

		player.setInitialPosition(currentPosX, currentPosY, currentAngleDegrees);
		player.init(currentLife, currentStamina, currentXp);
	}

}

