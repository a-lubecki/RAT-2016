using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

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

	
	private List<GameElementSaver> saveQueue = new List<GameElementSaver>();

	private void addToQueue(GameElementSaver saver) {

		saveQueue.Add(saver);

		//TODO notify thread
		processQueue();//TODO TEST
	}

	private void processQueue() {

		if(saveQueue.Count <= 0) {
			return;
		}

		//remove
		GameElementSaver saver = saveQueue[0];
		saveQueue.RemoveAt(0);

		//process
		saver.saveData();
	}


	public void saveCurrentLevel() {
		addToQueue(newSaverCurrentLevel());
	}
	
	public bool loadCurrentLevel() {
		return newSaverCurrentLevel().loadData();
	}
	
	public void savePlayerStats() {
		addToQueue(newSaverPlayerStats());
	}
	
	public bool loadPlayerStats() {
		return newSaverPlayerStats().loadData();
	}
	
	public void savePlayer() {
		addToQueue(newSaverPlayer());
	}
	
	public bool loadPlayer() {
		return newSaverPlayer().loadData();
	}

	public void deletePlayer() {
		newSaverPlayer().deleteData();
	}
	
	public void saveHub() {
		addToQueue(newSaverHub());
	}
	
	public bool loadHub() {
		return newSaverHub().loadData();
	}
	
	public void saveDoors() {
		addToQueue(newSaverDoors());
	}
	
	public bool loadDoors() {
		return newSaverDoors().loadData();
	}
	
	public void saveNpcs() {
		addToQueue(newSaverNcps());
	}
	
	public bool loadNpcs() {
		return newSaverNcps().loadData();
	}

	public void deleteNpcs() {
		newSaverNcps().deleteData();
	}

	public void saveListenerEvents() {
		addToQueue(newSaverListenerEvents());
	}
	
	public bool loadListenerEvents() {
		return newSaverListenerEvents().loadData();
	}


	
	private GameElementSaver newSaverCurrentLevel() {
		return new SaverCurrentLevelV1();
	}
	
	private GameElementSaver newSaverPlayerStats() {
		return new SaverPlayerStatsV1();
	}
	
	private GameElementSaver newSaverPlayer() {
		return new SaverPlayerV1();
	}
	
	private GameElementSaver newSaverHub() {
		return new SaverHubV1();
	}
	
	private GameElementSaver newSaverDoors() {
		return new SaverDoorsV1();
	}
	
	private GameElementSaver newSaverNcps() {
		return new SaverNpcsV1();
	}

	private GameElementSaver newSaverListenerEvents() {
		return new SaverListenerEventsV1();
	}
}

