using System;

[Serializable]
class NpcSaveData {
	
	public string id {get; private set; }
	
	private int currentPosX;
	private int currentPosY;
	private int currentAngleDegrees;
	
	private int currentLife;
	
	public NpcSaveData(Npc npc, AIControls aiControls) {
		
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

