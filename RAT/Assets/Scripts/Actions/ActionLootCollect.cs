using System;
using UnityEngine;

public class ActionLootCollect : BaseAction {

	public ActionLootCollect(Loot loot, bool enabled) 
		: base(loot,
			loot.getLootText() + "\n" + Constants.tr("Action.Loot.Collect"),
			false,
			enabled) {

	}

	
}

