using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class GameSaver {


	public bool loadCompleteGame() {

		bool ok = true;

		ok = ok && loadPlayer();
		//ok = ok && loadNpcs();
		//ok = ok && loadHubs();
		//ok = ok && loadDoors();

		return ok;
	}
	
	public bool saveCompleteGame() {
		
		bool ok = true;
		
		ok = ok && savePlayer();
		//ok = ok && saveNpcs();
		//ok = ok && saveHubs();
		//ok = ok && saveDoors();
		
		return ok;
	}


	public bool loadPlayer() {

		return new SaverPlayer().loadData();
	}
	
	public bool savePlayer() {
		
		return new SaverPlayer().saveData();
	}


	public bool loadNpcs() {
		
		return false;
	}
	
	public void saveNpcs() {
		
	}

	
	public bool loadHubs() {
		return false;
	}
	
	public void saveHubs() {
		
	}


	public bool loadDoors() {
		
		return false;
	}
	
	public void saveDoors() {
		
	}


}

