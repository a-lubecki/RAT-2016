using System;
using UnityEngine;

public class ActionDoorOpen : BaseAction {

	public ActionDoorOpen(DoorBehavior door) 
		: base(door,
			Constants.tr("Action.Door.Open")) {

	}

	
}

