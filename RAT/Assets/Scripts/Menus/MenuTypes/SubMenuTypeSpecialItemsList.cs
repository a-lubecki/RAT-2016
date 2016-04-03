using System;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuTypeSpecialItemsList : AbstractSubMenuType {

	public SubMenuTypeSpecialItemsList() : base("SpecialItemsList") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_SPECIAL_ITEMS_LIST;
	}


	public override List<string> getGridNames(bool topToBottom, bool leftToRight) {

		List<string> res = new List<string>();

		if(topToBottom) {

			if(leftToRight) {
				
				res.Add(Constants.GAME_OBJECT_NAME_GRID_DATA);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_KEYS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_GOALS);

			} else {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_KEYS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_DATA);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_GOALS);

			}

		} else {

			if(leftToRight) {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_GOALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_DATA);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_KEYS);

			} else {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_GOALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_KEYS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_DATA);

			}
		}
			
		return res;
	}

	public override bool isLeftGrid(string gridName) {
		return gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_DATA) ||
			gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_GOALS);
	}

}

