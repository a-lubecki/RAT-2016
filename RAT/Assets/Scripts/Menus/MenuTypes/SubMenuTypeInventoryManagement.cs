using System;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	public override List<string> getGridNames(bool topToBottom, bool leftToRight) {
		
		List<string> res = new List<string>();

		if(topToBottom) {
			
			if(leftToRight) {
				
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);

			} else {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);

			}

		} else {

			if(leftToRight) {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);

			} else {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
			}
		}

		return res;
	}

	public override bool isLeftGrid(string gridName) {
		return gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_BAG) ||
			gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_OBJECTS) ||
			gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_HEALS);
	}

}

