using System;

public class SubMenuTypeItemsChest : AbstractSubMenuType {

	public SubMenuTypeItemsChest() : base("ItemsChest") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_ITEMS_CHEST;
	}

}

