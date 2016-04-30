using System;
using UnityEngine;

public class ActionItemInGridSendToHub : BaseAction {

	public ActionItemInGridSendToHub(ItemInGrid itemInGrid, bool enabled) 
		: base(itemInGrid, 
			Constants.tr("Action.ItemInGrid.SendToHub"), 
			true, 
			enabled) {

	}


}

