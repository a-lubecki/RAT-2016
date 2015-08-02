using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaverPlayerV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "player";
	}
	
	public override bool isLevelSpecific() {
		return false;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}


	private PlayerData unserializedPlayerData;

	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		unserializedPlayerData = (PlayerData) bf.Deserialize(f);
	}

	protected override void assignUnserializedElement() {
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();
		
		unserializedPlayerData.assign(
			playerGameObject.GetComponent<Player>(),
			playerGameObject.GetComponent<PlayerControls>());
	}


	protected override bool serializeElement(BinaryFormatter bf, FileStream f) {

		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();

		PlayerData playerData = new PlayerData(
			playerGameObject.GetComponent<Player>(),
			playerGameObject.GetComponent<PlayerControls>());

		bf.Serialize(f, playerData);
		
		return true;
	}

}

[Serializable]
class PlayerData {

	private int currentPosX;
	private int currentPosY;
	private int currentAngleDegrees;
	
	private int currentLife;
	private int currentStamina;

	public PlayerData(Player player, PlayerControls playerControls) {
		
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
	}
	
	public void assign(Player player, PlayerControls playerControls) {
		
		if(player == null) {
			throw new System.ArgumentException();
		}
		if(playerControls == null) {
			throw new System.ArgumentException();
		}

		playerControls.setInitialPosition(currentPosX, currentPosY, currentAngleDegrees);
		
		player.init(currentLife, currentStamina);
	}

}

