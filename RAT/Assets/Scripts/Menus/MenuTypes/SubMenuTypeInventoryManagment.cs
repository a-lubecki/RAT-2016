using System;

public class SubMenuTypeInventoryManagment : AbstractSubMenuType {

	public SubMenuTypeInventoryManagment() : base("InventoryManagment") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

}

