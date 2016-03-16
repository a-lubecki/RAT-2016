using System;
using System.Collections.Generic;
using UnityEngine;
using Node;

public class Spawn : ISpawnable {
	
	public int posX { get ; private set; }
	public int posY { get ; private set; }
	public Direction direction { get ; private set; }


	public Spawn() : this(0, 0, Direction.UP) {

	}

	public Spawn(NodeElementSpawn nodeElementSpawn) 
		: this(nodeElementSpawn.nodePosition.x, 
			nodeElementSpawn.nodePosition.y, 
			nodeElementSpawn.nodeDirection.value) {

	}

	public Spawn(int posX, int posY, Direction direction) : base() {

		this.posX = posX;
		this.posY = posY;
		this.direction = direction;

	}

	int ISpawnable.getNextPosX() {
		return posX;
	}

	int ISpawnable.getNextPosY() {
		return posY;
	}

	Direction ISpawnable.getNextDirection() {
		return direction;
	}

}

