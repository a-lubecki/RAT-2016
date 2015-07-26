using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SaverNpcsV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "npcs";
	}
	
	public override bool isLevelSpecific() {
		return true;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}


	private NpcsListData unserializedNpcsData;

	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		unserializedNpcsData = (NpcsListData) bf.Deserialize(f);
	}

	protected override void assignUnserializedElement() {
		
		Npc[] npcs = GameHelper.Instance.getNpcs();
		
		unserializedNpcsData.assign(npcs);
	}


	protected override bool serializeElement(BinaryFormatter bf, FileStream f) {
		
		Npc[] npcs = GameHelper.Instance.getNpcs();

		NpcsListData npcsData = new NpcsListData(npcs);

		bf.Serialize(f, npcsData);
		
		return true;
	}

}

[Serializable]
class NpcsListData {

	private List<NpcData> npcsData = new List<NpcData>(); 
	
	public NpcsListData(Npc[] npcs) {

		foreach(Npc npc in npcs) {

			npcsData.Add(new NpcData(
				npc,
				npc.getAIControls()
				));
		}
	}

	public void assign(Npc[] npcs) {

		Dictionary<string, Npc> npcsById = new Dictionary<string, Npc>(npcs.Length);
		foreach(Npc npc in npcs) {
			npcsById.Add(npc.nodeElementNpc.nodeId.value, npc);
		}
		
		foreach(NpcData npcData in npcsData) {
			Npc npc = npcsById[npcData.id];
			npcData.assign(
				npc,
				npc.getAIControls()
				);
		}

	}
}

[Serializable]
class NpcData {
	
	public string id {get; private set; }

	private int currentPosX;
	private int currentPosY;
	private int currentAngleDegrees;
	
	private int currentLife;

	public NpcData(Npc npc, AIControls aiControls) {
		
		if(npc == null) {
			throw new System.ArgumentException();
		}
		if(aiControls == null) {
			throw new System.ArgumentException();
		}

		id = npc.nodeElementNpc.nodeId.value;

		currentPosX = (int)(aiControls.transform.position.x);
		currentPosY = (int)(aiControls.transform.position.y);
		currentAngleDegrees = (int)aiControls.angleDegrees;
		
		currentLife = npc.life;
	}
	
	public void assign(Npc npc, AIControls aiControls) {
		
		if(npc == null) {
			throw new System.ArgumentException();
		}
		if(aiControls == null) {
			throw new System.ArgumentException();
		}

		aiControls.setInitialPosition(currentPosX, currentPosY, currentAngleDegrees);
		
		npc.init(currentLife);
	}

}

