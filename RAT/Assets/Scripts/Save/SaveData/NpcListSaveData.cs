using System;
using System.Collections.Generic;

[Serializable]
class NpcListSaveData {

	private List<NpcSaveData> npcsData = new List<NpcSaveData>(); 
	
	public NpcListSaveData(Npc[] npcs) {
		
		if(npcs == null || npcs.Length <= 0) {
			return;
		}

		foreach(Npc npc in npcs) {
			npcsData.Add(new NpcSaveData(npc));
		}
	}

	public void assign(Npc[] npcs) {
		
		if(npcs == null || npcs.Length <= 0) {
			return;
		}

		Dictionary<string, Npc> npcsById = new Dictionary<string, Npc>(npcs.Length);
		foreach(Npc npc in npcs) {
			npcsById.Add(npc.nodeElementNpc.nodeId.value, npc);
		}
		
		foreach(NpcSaveData npcData in npcsData) {
			Npc npc = npcsById[npcData.id];
			npcData.assign(npc);
		}

	}
}
