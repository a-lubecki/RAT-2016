using System;

[Serializable]
class PlayerSaveData {

	private int currentPosX;
	private int currentPosY;
	private int currentAngleDegrees;
	
	private int currentLife;
	private int currentStamina;
	private int currentXp;

	public PlayerSaveData(Player player, PlayerCollider playerControls) {
		
		if(player == null) {
			throw new System.ArgumentException();
		}
		if(playerControls == null) {
			throw new System.ArgumentException();
		}

		currentPosX = (int)(playerControls.transform.position.x);
		currentPosY = (int)(playerControls.transform.position.y);
		currentAngleDegrees = (int)playerControls.angleDegrees;
		
		currentLife = player.life;
		currentStamina = player.stamina;
		currentXp = player.xp;
	}
	
	public void assign(Player player, PlayerCollider playerControls) {
		
		if(player == null) {
			throw new System.ArgumentException();
		}
		if(playerControls == null) {
			throw new System.ArgumentException();
		}

		playerControls.setInitialPosition(currentPosX, currentPosY, currentAngleDegrees);
		
		player.init(currentLife, currentStamina, currentXp);
	}

}

