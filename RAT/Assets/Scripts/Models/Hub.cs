using System;
using System.Collections.Generic;
using Node;

public class Hub : BaseListenerModel, ISpawnable {

	public static readonly string LISTENER_CALL_onHubActivated = "onHubActivated";
	public static readonly string LISTENER_CALL_onHubDeactivated = "onHubDeactivated";

	public int posX { get; private set; }
	public int posY { get; private set; }
	public Direction spawnDirection { get; private set; }
	public bool isActivated { get; private set; }


	public Hub(NodeElementHub nodeElementHub, bool isActivated) 
		: this(BaseListenerModel.getListeners(nodeElementHub), 
			nodeElementHub.nodePosition.x,
			nodeElementHub.nodePosition.y,
			isActivated) {
		
	}

	public Hub(List<Listener> listeners, int posX, int posY, bool isActivated) : base(listeners) {

		this.posX = posX;
		this.posY = posY;
		this.isActivated = isActivated;

	}

	int ISpawnable.getNextPosX() {
		return posX;
	}

	int ISpawnable.getNextPosY() {
		return posY;
	}

	Direction ISpawnable.getNextDirection() {
		return spawnDirection;
	}

	public void setActivated(bool isActivated) {
		this.isActivated = isActivated;
	}

}

