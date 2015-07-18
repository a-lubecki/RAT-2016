using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class GameSaver {

	private static GameSaver instance;
	
	private GameSaver() {}
	
	public static GameSaver Instance {
		
		get {
			if (instance == null) {
				instance = new GameSaver();
			}
			return instance;
		}
	}


	public GameElementSaver getSaverCurrentLevel() {
		return new SaverCurrentLevelV1();
	}
	
	public GameElementSaver getSaverPlayerStats() {
		return new SaverPlayerStatsV1();
	}

	public GameElementSaver getSaverPlayerPosition() {
		return new SaverPlayerPositionV1();
	}

	public GameElementSaver getSaverNcps() {
		return null;//TODO
	}

	public GameElementSaver getSaverHubs() {
		return null;//TODO
	}

	public GameElementSaver getSaverDoors() {
		return null;//TODO
	}


}

