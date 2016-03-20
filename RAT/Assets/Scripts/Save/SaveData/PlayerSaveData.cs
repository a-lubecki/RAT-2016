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

		PlayerBehavior playerBehavior = GameHelper.Instance.findPlayerBehavior();
		if(playerBehavior != null) {
			currentPosX = (int)(playerBehavior.transform.position.x);
			currentPosY = (int)(playerBehavior.transform.position.y);
		} else {
			currentPosX = player.initialPosX;
			currentPosY = player.initialPosY;
		}

		currentAngleDegrees = player.angleDegrees;
		
		currentLife = player.life;
		currentStamina = player.stamina;
		currentXp = player.xp;
	}


}

