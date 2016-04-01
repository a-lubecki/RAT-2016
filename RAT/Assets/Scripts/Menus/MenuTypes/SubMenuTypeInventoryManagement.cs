using System;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	public override List<string> getGridNames() {
		
		List<string> res = new List<string>();

		res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
		res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);

		return res;
	}

}

