using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaverPlayerPositionV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "playerPos";
	}
	
	public override bool isLevelSpecific() {
		return false;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}

	
	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		PlayerPositionData playerPositionData = (PlayerPositionData) bf.Deserialize(f);
		
		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();

		playerPositionData.assign(
		    playerGameObject.GetComponent<PlayerControls>());
	}
	
	protected override void serializeElement(BinaryFormatter bf, FileStream f) {

		GameObject playerGameObject = GameHelper.Instance.getPlayerGameObject();

		PlayerPositionData playerPositionData = new PlayerPositionData(
			playerGameObject.GetComponent<PlayerControls>());

		bf.Serialize(f, playerPositionData);

	}

}

[Serializable]
class PlayerPositionData {

	private int currentPosX;
	private int currentPosY;
	private int currentAngleDegrees;

	public PlayerPositionData(PlayerControls playerControls) {

		if(playerControls == null) {
			throw new System.ArgumentException();
		}

		currentPosX = (int)(playerControls.transform.position.x / (float) Constants.TILE_SIZE);
		currentPosY = - (int)(playerControls.transform.position.y / (float) Constants.TILE_SIZE);
		currentAngleDegrees = (int)playerControls.angleDegrees;
	}
	
	public void assign(PlayerControls playerControls) {

		if(playerControls == null) {
			throw new System.ArgumentException();
		}

		playerControls.setInitialPosition(currentPosX, currentPosY, currentAngleDegrees);
	}

}

