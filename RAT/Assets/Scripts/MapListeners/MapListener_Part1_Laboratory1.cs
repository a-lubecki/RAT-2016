using System;
using UnityEngine;
using Node;
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
		GameSaver.Instance.saveAllToFile();

		//wait to let the 'hub activated' message disappear
		yield return new WaitForSeconds(3f);

		openDoors(true);

		//display a message to notify the player
		MessageDisplayer.Instance.displayMessages(new Message(Constants.tr("Message.Event.DoorsOpened")));

	}


	private void openDoors(bool animated) {
		
		DoorBehavior[] doorsBehaviors = GameObject.FindObjectsOfType<DoorBehavior>();
		
		//find the 3 doors
		HashSet<string> doorsIds = new HashSet<string>();
		doorsIds.Add("door11");
		doorsIds.Add("door12");
		doorsIds.Add("door13");
		
		List<DoorBehavior> selectedDoors = new List<DoorBehavior>();
		
		foreach(DoorBehavior doorBehavior in doorsBehaviors) {
			
			string doorId = doorBehavior.door.id;
			bool found = false;
			
			foreach(string id in doorsIds) {
				
				if(doorId.Equals(id)) {
					found = true;
					break;
				}
			}
			
			if(found) {
				
				selectedDoors.Add(doorBehavior);
				
				doorsIds.Remove(doorId);
				
				if(doorsIds.Count <= 0) {
					//all found
					break;
				}
			}
			
		}
		
		//open the selected doors
		foreach(DoorBehavior door in selectedDoors) {
			door.open(animated);
		}

	}

}

