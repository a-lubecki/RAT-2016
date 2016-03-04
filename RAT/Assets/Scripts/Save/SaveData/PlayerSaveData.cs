using System;

[Serializable]
public class PlayerSaveData {
	
	protected int currentPosX;
	protected int currentPosY;
	protected int currentAngleDegrees;
	
	protected int currentLife;
	protected int currentStamina;
	protected int currentXp;
	
	public int getCurrentPosX() {
		return currentPosX;
	}
	public int getCurrentPosY() {
		return currentPosY;
	}
	public int getCurrentAngleDegrees() {
		return currentAngleDegrees;
	}
	public int getCurrentLife() {
		return currentLife;
	}
	public int getCurrentStamina() {
		return currentStamina;
	}
	public int getCurrentXp() {
		return currentXp;
	}

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

