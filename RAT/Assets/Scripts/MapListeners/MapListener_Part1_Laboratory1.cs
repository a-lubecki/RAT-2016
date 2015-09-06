using System;
using UnityEngine;
using Level;
using System.Collections;
using System.Collections.Generic;

public class MapListener_Part1_Laboratory1 : MonoBehaviour, IMapListener {
	
	private static readonly string EVENT_HUB_ACTIVATED = "onFirstHubActivated"; 


	private Dictionary<string, bool> achievedEvents = new Dictionary<string, bool>();

	string[] IMapListener.getEventIds() {
		return new string[] { EVENT_HUB_ACTIVATED };
	}

	bool IMapListener.isEventAchieved(string eventId) {
		if(!achievedEvents.ContainsKey(eventId)) {
			return false;
		}
		return achievedEvents[eventId];
	}
	
	void IMapListener.achieveEvent(string eventId) {

		achievedEvents[eventId] = true;

		if(eventId.Equals(EVENT_HUB_ACTIVATED)) {
			openDoors(false);
		}
	}


	/**
	 * Open the 3 doors after a delay
	 */
	public IEnumerator onFirstHubActivated() {

		achievedEvents[EVENT_HUB_ACTIVATED] = true;

		GameSaver.Instance.saveListenerEvents();

		//wait to let the 'hub activated' message disappear
		yield return new WaitForSeconds(3f);

		openDoors(true);

		//display a message to notify the player
		MessageDisplayer.Instance.displayMessages(new Message("Les portes se sont ouvertes"));
	}


	private void openDoors(bool animated) {
		
		Door[] doors = GameObject.FindObjectsOfType<Door>();
		
		//find the 3 doors
		HashSet<string> doorsIds = new HashSet<string>();
		doorsIds.Add("door11");
		doorsIds.Add("door12");
		doorsIds.Add("door13");
		
		List<Door> selectedDoors = new List<Door>();
		
		foreach(Door door in doors) {
			
			NodeString nodeId = door.nodeElementDoor.nodeId;
			
			if(nodeId == null) {
				continue;
			}
			
			string doorId = nodeId.value;
			bool found = false;
			
			foreach(string id in doorsIds) {
				
				if(doorId.Equals(id)) {
					found = true;
					break;
				}
			}
			
			if(found) {
				
				selectedDoors.Add(door);
				
				doorsIds.Remove(doorId);
				
				if(doorsIds.Count <= 0) {
					//all found
					break;
				}
			}
			
		}
		
		//open the selected doors
		foreach(Door door in selectedDoors) {
			door.open(animated);
		}

	}

}

