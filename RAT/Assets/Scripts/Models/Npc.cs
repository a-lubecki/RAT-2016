using System;
using System.Collections.Generic;
using Node;

public class Npc : Character {

	private static int getMaxLife(int level) {
		//TODO set constant with npc type
		return level * 100;
	}

	public int level { get; private set; }

	public Npc(NodeElementNpc nodeElementNpc, bool setLife, int life, int angleDegrees) 
		: this(nodeElementNpc.nodeId.value,
			BaseListenerModel.getListeners(nodeElementNpc),
			nodeElementNpc.nodeLevel.value, 
			setLife,
			life,
			nodeElementNpc.nodePosition.x,
			nodeElementNpc.nodePosition.y,
			nodeElementNpc.nodeDirection.value,
			angleDegrees) {

	}

	public Npc(string id, List<Listener> listeners, int level, bool setLife, int life, int initialPosX, int initialPosY, CharacterDirection initialDirection, int angleDegrees) 
		: base(id, 
			listeners,
			getMaxLife(level), 
			setLife ? life : getMaxLife(level), 
			initialPosX,
			initialPosY,
			initialDirection,
			angleDegrees) {

		if(level <= 0) {
			throw new ArgumentException();
		}

		this.level = level;

	}


}

