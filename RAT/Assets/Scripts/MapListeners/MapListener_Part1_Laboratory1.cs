using System;
using UnityEngine;
using Level;
using System.Collections;
using System.Collections.Generic;

public class MapListener_Part1_Laboratory1 : MonoBehaviour, IMapListener {

	/**
	 * Open the 3 doors after a delay
	 */
	public IEnumerator onFirstHubActivated() {

		//wait to let the 'hub activated' message disappear
		yield return new WaitForSeconds(3f);

		Door[] doors = GameObject.FindObjectsOfType<Door>();

		//find the 3 doors
		HashSet<string> doorsIds = new HashSet<string>();
		doorsIds.Add("door1");
		doorsIds.Add("door2");
		doorsIds.Add("door3");

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
			door.open(true);
		}

		//display a message to notify the player
		MessageDisplayer.Instance.displayMessage("Les portes se sont ouvertes");
	}

}

