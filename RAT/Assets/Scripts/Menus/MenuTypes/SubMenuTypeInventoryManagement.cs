using System;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	private GameObject getGameObject() {

		GameObject subMenuKeeper = GameHelper.Instance.getSubMenuKeeper();

		Transform subMenuTransform = subMenuKeeper.transform.Find(getGameObjectName());
		if(subMenuTransform == null) {
			subMenuTransform = GameHelper.Instance.getMenu().transform.Find(getGameObjectName());
			if(subMenuTransform == null) {
				throw new NotSupportedException("GameObject not found");
			}
		}

		return subMenuTransform.gameObject;

	}

	public override void updateViews(GameObject gameObjectSubMenu) {
		
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_BAG);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_HEALS);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
		updateGrid(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);

	}

	private void updateGrid(string gridName) {

		InventoryGrid inventoryGrid = findInventoryGrid(gridName);

		inventoryGrid.deleteGridViews();
		inventoryGrid.updateGridViews();

		inventoryGrid.removeItems();
		inventoryGrid.addItems(GameManager.Instance.getInventory().getItems(gridName));
	}

	public InventoryGrid findInventoryGrid(string gridGameObjectName) {

		Transform transformGridBag = getGameObject().transform.Find(gridGameObjectName);
		return transformGridBag.GetComponent<InventoryGrid>();
	}

	public bool isNewItemFitting(string gridName, ItemPattern itemPattern, int nbGrouped) {

		if(string.IsNullOrEmpty(gridName)) {
			throw new ArgumentException();
		}
		if(itemPattern == null) {
			throw new ArgumentException();
		}

		Inventory inventory = GameManager.Instance.getInventory();

		//check if item pattern already exist and can be merged
		ItemInGrid itemWithPattern = inventory.getAnyItemInGridWithPattern(itemPattern);
		if(itemWithPattern != null && 
			nbGrouped + itemWithPattern.getNbGrouped() <= itemPattern.maxGroupable) {
			return true;
		}

		return (getNewItemCoords(gridName, itemPattern) != null);
	}


	public int[] getNewItemCoords(string gridName, ItemPattern itemPattern) {

		if(string.IsNullOrEmpty(gridName)) {
			throw new ArgumentException();
		}
		if(itemPattern == null) {
			throw new ArgumentException();
		}

		Inventory inventory = GameManager.Instance.getInventory();
		InventoryGrid grid = findInventoryGrid(gridName);

		//check for taken blocks

		int wGrid = grid.width;
		int hGrid = grid.height;

		bool[,] takenBlocks = new bool[wGrid, hGrid];

		foreach(ItemInGrid item in inventory.getItems(gridName)) {

			int posX = item.getPosXInBlocks();
			int posY = item.getPosYInBlocks();
			int w = item.getItemPattern().widthInBlocks;
			int h = item.getItemPattern().heightInBlocks;

			for(int y = 0 ; y < h ; y++) {

				if(y >= hGrid) {
					break;
				}

				for(int x = 0 ; x < w ; x++) {

					if(x >= wGrid) {
						break;
					}

					takenBlocks[posX + x, posY + y] = true;
				}
			}

		}

		//look if matches free blocks

		int wItem = itemPattern.widthInBlocks;
		int hItem = itemPattern.heightInBlocks;

		int minSize = (wItem < hItem) ? wItem : hItem;

		int wGridMinusSize = wGrid - minSize;
		int hGridMinusSize = hGrid - minSize;

		for(int j = 0 ; j < hGridMinusSize ; j++) {
			for(int i = 0 ; i < wGridMinusSize ; i++) {

				if(!takenBlocks[i, j]) {

					bool hasAllFreeBlocks = true;

					//check for non taken blocks in the itempatterns ranges horizontally
					for(int y = 0 ; y < hItem; y++) {

						if(j + y >= hGrid) {
							hasAllFreeBlocks = false;
							break;
						}

						for(int x = 0 ; x < wItem; x++) {

							if(i + x >= wGrid) {
								hasAllFreeBlocks = false;
								break;
							}

							if(takenBlocks[i + x, j + y]) {
								hasAllFreeBlocks = false;
								break;
							}
						}

						if(!hasAllFreeBlocks) {
							break;
						}
					}

					if(hasAllFreeBlocks) {
						return new int[] { i, j };
					}

					if(wItem != hItem) {
						break;
					}

					hasAllFreeBlocks = true;

					//check for non taken blocks in the itempatterns ranges vertically
					for(int y = 0 ; y < wItem; y++) {

						if(y >= hGrid) {
							hasAllFreeBlocks = false;
							break;
						}

						for(int x = 0 ; x < hItem; x++) {

							if(x >= wGrid) {
								hasAllFreeBlocks = false;
								break;
							}

							if(takenBlocks[i + x, j + y]) {
								hasAllFreeBlocks = false;
								break;
							}
						}

						if(!hasAllFreeBlocks) {
							break;
						}
					}

					if(hasAllFreeBlocks) {
						return new int[] { i, j };
					}

				}
			}
		}

		return null;
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

