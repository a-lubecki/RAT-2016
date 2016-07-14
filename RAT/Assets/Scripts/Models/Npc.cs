using System;
using System.Collections.Generic;
using Node;
using UnityEngine;

public class Npc : Character {

	private static int getMaxLife(int level) {
		//TODO set constant with npc type
		return level * 100;
	}

	public int level { get; private set; }

	public Npc(NodeElementNpc nodeElementNpc, bool setLife, int life, int realPosX, int realPosY, int angleDegrees) 
		: this(nodeElementNpc.nodeId.value,
			BaseListenerModel.getListeners(nodeElementNpc),
			nodeElementNpc.nodeLevel.value, 
			setLife,
			life,
			nodeElementNpc.nodePosition.x,
			nodeElementNpc.nodePosition.y,
			nodeElementNpc.nodeDirection.value,
			realPosX,
			realPosY,
			angleDegrees) {

	}

	public Npc(string id, List<Listener> listeners, int level, bool setLife, int life,
	int initialMapPosX, int initialMapPosY, CharacterDirection initialDirection, int realPosX, int realPosY, int angleDegrees)  
		: base(id, 
			listeners,
			getMaxLife(level), 
			setLife ? life : getMaxLife(level),
			initialMapPosX,
			initialMapPosY, 
			initialDirection,
			realPosX,
			realPosY,
			angleDegrees) {

		if(level <= 0) {
			throw new ArgumentException();
		}

		this.level = level;

	}


	public void reinitLifeAndPosition() {

		updateRealPositionAngle(
			false, 
			new Vector2(initialMapPosX * Constants.TILE_SIZE, - initialMapPosY * Constants.TILE_SIZE),
			Character.directionToAngle(initialDirection));

		reinitLife();
	}

	protected override void onDie() {

		GameHelper.Instance.getPlayer().earnXp(500);//TODO test
	}


	public override Vector2 getNewMoveVector() {

		return new Vector2(0, 0);//TODO
	}

	protected override bool canRun() {
		return true;
	}

	protected override CharacterAction getCurrentCharacterAction() {
		return new CharacterAction(false, 100);
	}

	protected override BaseCharacterState getNextState() {
		return BaseCharacterState.WAIT;
	}

}

