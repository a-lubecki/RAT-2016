using System;
using UnityEngine;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	public override void updateViews(GameObject gameObjectSubMenu) {
		
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_BAG);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_HEALS);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);

	}


	public override void validate() {
	}
	
	public override void cancel() {
	}
	
	public override void navigateUp() {
	}
	
	public override void navigateDown() {
	}
	
	public override void navigateRight() {
	}
	
	public override void navigateLeft() {
	}

}

