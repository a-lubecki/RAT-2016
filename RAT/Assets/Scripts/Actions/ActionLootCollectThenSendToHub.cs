using System;
using UnityEngine;

public class ActionLootCollectThenSendToHub : BaseAction {

	public ActionLootCollectThenSendToHub(LootBehavior lootBehavior) 
		: base(lootBehavior, 
			lootBehavior.loot.getLootText() + "\n" + Constants.tr("Action.Loot.CollectThenSendToHub"),
			true, 
			true) {

	}

	
}

