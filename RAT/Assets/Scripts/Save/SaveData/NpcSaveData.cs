using System;

[Serializable]
public class NpcSaveData {
	
	protected string id;
	
	protected int currentPosX;
	protected int currentPosY;
	protected int currentAngleDegrees;
	
	protected int currentLife;

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

		id = npc.id;

		NpcBehavior npcBehavior = GameHelper.Instance.findNpcBehavior(npc);
		if(npcBehavior != null) {
			currentPosX = (int)(npcBehavior.transform.position.x);
			currentPosY = (int)(npcBehavior.transform.position.y);
		} else {
			currentPosX = npc.initialPosX;
			currentPosY = npc.initialPosY;
		}

		currentAngleDegrees = npc.angleDegrees;
		currentLife = npc.life;
	}

}

