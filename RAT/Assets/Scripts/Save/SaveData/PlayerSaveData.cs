using System;
using UnityEngine;

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

		GameObject playerGameObject = player.findGameObject<PlayerBehavior>();
		if(playerGameObject != null) {
			currentPosX = (int)(playerGameObject.transform.position.x);
			currentPosY = (int)(playerGameObject.transform.position.y);
		} else {
			currentPosX = player.initialMapPosX;
			currentPosY = player.initialMapPosY;
		}

		currentAngleDegrees = player.angleDegrees;
		
		currentLife = player.life;
		currentStamina = player.stamina;
		currentXp = player.xp;
	}


}

