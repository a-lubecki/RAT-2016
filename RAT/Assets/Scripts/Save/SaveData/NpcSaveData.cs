using System;

[Serializable]
public class NpcSaveData {
	
	private string id;
	
	private int currentPosX;
	private int currentPosY;
	private int currentAngleDegrees;
	
	private int currentLife;

	public string getId() {
		return id;
	}
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

