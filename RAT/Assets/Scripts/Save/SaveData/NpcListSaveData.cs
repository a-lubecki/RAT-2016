using System;
using System.Collections.Generic;

[Serializable]
public class NpcListSaveData {

	private List<NpcSaveData> npcsData = new List<NpcSaveData>(); 
	
	public NpcListSaveData(Npc[] npcs) {
		
		if(npcs == null) {
			return;
		}
		
		if(npcs.Length <= 0) {
			npcsData.Clear();
			return;
		}

		foreach(Npc npc in npcs) {
			npcsData.Add(new NpcSaveData(npc));
		}
	}

	
	public Dictionary<string, NpcSaveData> getNpcsDataById() {
		
		Dictionary<string, NpcSaveData> npcsById = new Dictionary<string, NpcSaveData>(npcsData.Count);
		
		foreach(NpcSaveData npcData in npcsData) {
			npcsById.Add(npcData.getId(), npcData);
		}
		
		return npcsById;
	}

}
