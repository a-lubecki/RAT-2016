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

	public bool isNewItemFitting(string gridName, ItemPattern itemPattern) {

		InventoryGrid grid = findInventoryGrid(gridName);
		return grid.isItemPatternFitting(itemPattern);
	}

	public bool isNewItemCollectible(string gridName, ItemPattern itemPattern) {

		if(getGroupableItem(gridName, itemPattern) != null) {
			return true;
		}
		if(getNewItemCoords(gridName, itemPattern) != null) {
			return true;
		}

		return false;
	}


	public ItemInGrid getGroupableItem(string gridName, ItemPattern itemPattern) {

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
			itemWithPattern.getNbGrouped() < itemPattern.maxGroupable) {
			return itemWithPattern;
		}

		return null;
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

		bool[,] takenBlocks = new bool[hGrid, wGrid];

		foreach(ItemInGrid item in inventory.getItems(gridName)) {

			int posX = item.getPosXInBlocks();
			int posY = item.getPosYInBlocks();
			int w;
			int h;
			if(item.getOrientation() == Orientation.FACE) {
				w = item.getItemPattern().widthInBlocks;
				h = item.getItemPattern().heightInBlocks;
			} else {
				w = item.getItemPattern().heightInBlocks;
				h = item.getItemPattern().widthInBlocks;
			}

			for(int y = 0 ; y < h ; y++) {

				if(y >= hGrid) {
					break;
				}

				for(int x = 0 ; x < w ; x++) {

					if(x >= wGrid) {
						break;
					}

					takenBlocks[posY + y, posX + x] = true;
				}
			}

		}

		//look if matches free blocks

		int wItem = itemPattern.widthInBlocks;
		int hItem = itemPattern.heightInBlocks;

		int minSize = (wItem < hItem) ? wItem : hItem;

		int wGridMinusSize = wGrid - minSize + 1;
		int hGridMinusSize = hGrid - minSize + 1;

		for(int j = 0 ; j < hGridMinusSize ; j++) {
			for(int i = 0 ; i < wGridMinusSize ; i++) {

				if(!takenBlocks[j, i]) {

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

							if(takenBlocks[j + y, i + x]) {
								hasAllFreeBlocks = false;
								break;
							}
						}

						if(!hasAllFreeBlocks) {
							break;
						}
					}

					if(hasAllFreeBlocks) {
						return new int[] { i, j, (int) Orientation.FACE };
					}

					if(wItem == hItem) {
						//same size, no need to test with another orientation	
						break;
					}

					//reset flag
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

							if(takenBlocks[j + y, i + x]) {
								hasAllFreeBlocks = false;
								break;
							}
						}

						if(!hasAllFreeBlocks) {
							break;
						}
					}

					if(hasAllFreeBlocks) {
						return new int[] { i, j, (int)Orientation.SIDE };
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

