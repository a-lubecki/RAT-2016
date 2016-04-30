using System;
using UnityEngine;

public class ActionItemInGridCast : BaseAction {

	public ActionItemInGridCast(ItemInGrid itemInGrid, bool enabled) 
		: base(itemInGrid, 
			Constants.tr("Action.ItemInGrid.Cast"), 
			true, 
			enabled) {

	}


}

