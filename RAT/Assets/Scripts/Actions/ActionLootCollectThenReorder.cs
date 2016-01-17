using System;
using UnityEngine;

public class ActionLootCollectThenReorder : BaseAction {

	public ActionLootCollectThenReorder(Loot loot) : base(loot, loot.getLootText() + "\n" + Constants.tr("Action.Loot.CollectThenReorder")) {

	}

	
}

