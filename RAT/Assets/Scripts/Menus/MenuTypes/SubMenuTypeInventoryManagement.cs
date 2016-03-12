using System;
using UnityEngine;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	
	public override void updateViews(GameObject gameObjectSubMenu) {

		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_BAG).updateGridViews();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_OBJECTS).updateGridViews();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_HEALS).updateGridViews();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT).updateGridViews();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_EQUIP).updateGridViews();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT).updateGridViews();

	}

	private InventoryGrid findInventoryGrid(GameObject parentGameObject, string gridGameObjectName) {
		
		Transform transformGridBag = parentGameObject.transform.Find(gridGameObjectName);
		return transformGridBag.GetComponent<InventoryGrid>();
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

