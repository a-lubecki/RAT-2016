using System;

[Serializable]
public class NpcSaveData {
	
	public string id { get; private set; }
	
	public int currentPosX { get; private set; }
	public int currentPosY { get; private set; }
	public int currentAngleDegrees { get; private set; }
	
	public int currentLife { get; private set; }
	
	public NpcSaveData(Npc npc) {
		
		if(npc == null) {
			throw new System.ArgumentException();
		}
		
		id = npc.nodeElementNpc.nodeId.value;
		
		currentPosX = (int)(npc.transform.position.x);
		currentPosY = (int)(npc.transform.position.y);
		currentAngleDegrees = (int)npc.angleDegrees;
		
		currentLife = npc.life;
	}
	
	public void assign(Npc npc) {
		
		if(npc == null) {
			throw new System.ArgumentException();
		}
		
		npc.setInitialPosition(currentPosX, currentPosY, currentAngleDegrees);
		npc.init(currentLife);
	}
	
}

