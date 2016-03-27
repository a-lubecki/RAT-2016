using System;
using UnityEngine;

public class SubMenuTypeSpecialItemsList : AbstractSubMenuType {

	public SubMenuTypeSpecialItemsList() : base("SpecialItemsList") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_SPECIAL_ITEMS_LIST;
	}


	public override void updateViews(GameObject gameObjectSubMenu) {

		updateGrid(Constants.GAME_OBJECT_NAME_GRID_DATA);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_KEYS);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_GOALS);

	}

}

