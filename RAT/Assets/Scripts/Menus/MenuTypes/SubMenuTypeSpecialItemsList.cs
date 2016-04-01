using System;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuTypeSpecialItemsList : AbstractSubMenuType {

	public SubMenuTypeSpecialItemsList() : base("SpecialItemsList") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_SPECIAL_ITEMS_LIST;
	}


	public override List<string> getGridNames() {

		List<string> res = new List<string>();

		res.Add(Constants.GAME_OBJECT_NAME_GRID_DATA);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_KEYS);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_GOALS);

		return res;
	}

}

