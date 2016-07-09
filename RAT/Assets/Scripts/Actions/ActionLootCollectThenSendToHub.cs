using System;
using UnityEngine;

public class ActionLootCollectThenSendToHub : BaseAction {

	public ActionLootCollectThenSendToHub(Loot loot) 
		: base(loot, 
			loot.getLootText() + "\n" + Constants.tr("Action.Loot.CollectThenSendToHub"),
			true, 
			true) {

	}

	
}

