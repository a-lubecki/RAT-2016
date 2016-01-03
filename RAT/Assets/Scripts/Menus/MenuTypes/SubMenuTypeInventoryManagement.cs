using System;
using UnityEngine;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	
	protected override void buildInternal(GameObject gameObjectSubMenu) {
		
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_BAG).build();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_OBJECTS).build();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_HEALS).build();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT).build();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_EQUIP).build();
		findInventoryGrid(gameObjectSubMenu, Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT).build();

	}

	private InventoryGrid findInventoryGrid(GameObject parentGameObject, string gridGameObjectName) {
		
		Transform transformGridBag = parentGameObject.transform.Find(gridGameObjectName);
		return transformGridBag.GetComponent<InventoryGrid>();
	}

}

