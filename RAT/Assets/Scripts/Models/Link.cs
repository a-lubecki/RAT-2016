using System;
using System.Collections.Generic;
using UnityEngine;
using Node;

public class Link : BaseListenerModel, ISpawnable {
	
	public string nextMap { get ; private set; }
	public int nextPosX { get ; private set; }
	public int nextPosY { get ; private set; }
	public Direction nextDirection { get ; private set; }


	public Link(NodeElementLink nodeElementLink, string nextMapFallBack) 
		: this(BaseListenerModel.getListeners(nodeElementLink),
			nodeElementLink.nodeNextMap != null ? nodeElementLink.nodeNextMap.value : nextMapFallBack, 
			nodeElementLink.nodeNextPosition.x, 
			nodeElementLink.nodeNextPosition.y, 
			nodeElementLink.nodeNextDirection.value) {

	}

	public Link(List<Listener> listeners, string nextMap, int nextPosX, int nextPosY, Direction nextDirection) : base(listeners) {

		if(string.IsNullOrEmpty(nextMap)) {
			throw new ArgumentException();
		}

		this.nextMap = nextMap;
		this.nextPosX = nextPosX;
		this.nextPosY = nextPosY;
		this.nextDirection = nextDirection;

	}

	int ISpawnable.getNextPosX() {
		return nextPosX;
	}

	int ISpawnable.getNextPosY() {
		return nextPosY;
	}

	Direction ISpawnable.getNextDirection() {
		return nextDirection;
	}

}

