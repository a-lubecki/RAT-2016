using System;
using UnityEngine;

public class ActionLootCollectThenReorder : BaseAction {

	public ActionLootCollectThenReorder(LootBehavior lootBehavior) : base(lootBehavior, lootBehavior.loot.getLootText() + "\n" + Constants.tr("Action.Loot.CollectThenReorder")) {

	}

	
}

