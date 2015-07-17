using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaverPlayer : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "player";
	}

	
	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		
		if(player == null) {
			throw new System.InvalidOperationException("Player is null");
		}
		
		player.unserialize(bf, f);
	}
	
	protected override void serializeElement(BinaryFormatter bf, FileStream f) {
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		
		if(player == null) {
			throw new System.InvalidOperationException("Player is null");
		}

		player.serialize(bf, f);
	}
		

}

