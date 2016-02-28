using System;
using System.Collections.Generic;

[Serializable]
public class NpcListSaveData {

	private Dictionary<string, NpcSaveData> npcsDataById = new Dictionary<string, NpcSaveData>(); 
	
	public NpcListSaveData(Npc[] npcs) {
		
		if(npcs == null) {
			return;
		}

		foreach(Npc npc in npcs) {
			NpcSaveData npcsData = new NpcSaveData(npc);
			npcsDataById.Add(npcsData.getId(), npcsData);
		}
	}

	
	public Dictionary<string, NpcSaveData> getNpcsDataById() {
		return new Dictionary<string, NpcSaveData>(npcsDataById);
	}

}
