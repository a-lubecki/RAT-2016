using System;
using UnityEngine;

public class ActionLootCollect : BaseAction {

	public ActionLootCollect(LootBehavior lootBehavior) : base(lootBehavior, lootBehavior.loot.getLootText() + "\n" + Constants.tr("Action.Loot.Collect")) {

	}

	
}

