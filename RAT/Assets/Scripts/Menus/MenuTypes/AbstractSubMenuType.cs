using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSubMenuType : Displayable {

	public AbstractSubMenuType(string trKey) : base("SubMenuType." + trKey) {

	}

	public abstract string getGameObjectName();

	public GameObject getSubMenuGameObject(Menu menu) {

		if(menu == null) {
			throw new System.ArgumentException();
		}

		Transform transform = menu.transform.Find(getGameObjectName());
		if(transform == null) {
			return null;
		}

		return transform.gameObject;
	}

	public virtual void updateViews(GameObject gameObjectSubMenu) {
		
		foreach(string gridName in getGridNames(true, true)) {
			updateGrid(gridName);
		}

	}

	public virtual List<string> getGridNames(bool topToBottom, bool leftToRight) {
		return new List<string>();
	}

	protected void updateGrid(string gridName) {

		InventoryGrid inventoryGrid = findInventoryGrid(gridName);

		if(inventoryGrid == null) {
			return;
		}

		inventoryGrid.deleteGridViews();
		inventoryGrid.updateGridViews();

		inventoryGrid.removeItems();
		inventoryGrid.addItems(GameManager.Instance.getInventory().getItems(gridName));
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

	public InventoryGrid findInventoryGrid(string gridGameObjectName) {

		if(gridGameObjectName == null) {
			throw new ArgumentException();
		}

		Transform transformGrid = getGameObject().transform.Find(gridGameObjectName);
		if(transformGrid == null) {
			Debug.Log("Couldn't find grid transform : " + gridGameObjectName);
			return null;
		}

		InventoryGrid grid = transformGrid.GetComponent<InventoryGrid>();
		if(transformGrid == null) {
			Debug.Log("Couldn't find grid InventoryGrid component : " + gridGameObjectName);
			return null;
		}

		return grid;
	}

	public virtual bool isLeftGrid(string gridName) {
		return true;
	}

	public virtual void selectFirstItem(bool leftToRight) {
		
		//select first item we can find in grids
		ItemInGrid firstItem = null;
		int x = leftToRight ? int.MaxValue : 0;
		int y = int.MaxValue;

		List<string> gridNames = getGridNames(true, leftToRight);

		foreach(string gridName in gridNames) {
			
			List<ItemInGrid> items = GameManager.Instance.getInventory().getItems();
			foreach(ItemInGrid item in items) {

				if(!gridName.Equals(item.getGridName())) {
					continue;
				}

				if(leftToRight) {
					if(item.getPosYInBlocks() <= y && item.getPosXInBlocks() <= x) {
						firstItem = item;
						x = item.getPosXInBlocks();
						y = item.getPosYInBlocks();
					}
				} else {
					if(item.getPosYInBlocks() <= y && item.getPosXInBlocks() >= x) {
						firstItem = item;
						x = item.getPosXInBlocks();
						y = item.getPosYInBlocks();
					}
				}
			}

			if(firstItem != null) {
				break;
			}
		}

		if(firstItem != null) {
			GameHelper.Instance.getMenu().menuSelector.selectItem(this, firstItem);
		}

	}

	/**
	 * Return if the action is consumed by the submenu or if the menu must take over
	 */
	public virtual bool validate() {
		return false;
	}

	/**
	 * Return if the action is consumed by the submenu or if the menu must take over
	 */
	public virtual bool cancel() {
		return false;
	}

	/**
	 * Return if the action is consumed by the submenu or if the menu must take over
	 */
	public virtual bool navigateUp() {

		return navigate(Direction.UP);
	}

	/**
	 * Return if the action is consumed by the submenu or if the menu must take over
	 */
	public virtual bool navigateDown() {
		/*
		ISelectable selected = GameHelper.Instance.getMenu().menuSelector.selectedItem;
		if(selected != null && selected is MenuArrow) {

			MenuArrow menuArrow = selected as MenuArrow;
			if(menuArrow.isLeft) {

			} else {

			}

		}
		return false;
*/
		return navigate(Direction.DOWN);
	}

	/**
	 * Return if the action is consumed by the submenu or if the menu must take over
	 */
	public virtual bool navigateRight() {

		return navigate(Direction.RIGHT);
	}

	/**
	 * Return if the action is consumed by the submenu or if the menu must take over
	 */
	public virtual bool navigateLeft() {

		return navigate(Direction.LEFT);
	}

	private bool navigate(Direction direction) {

		MenuSelector menuSelector = GameHelper.Instance.getMenu().menuSelector;

		ISelectable currentSelectedItem = menuSelector.selectedItem;
		if(currentSelectedItem != null && currentSelectedItem is ItemInGrid) {

			menuSelector.selectItem(this, getNextItemInGrid(currentSelectedItem as ItemInGrid, direction));

			return (currentSelectedItem != menuSelector.selectedItem);
		}

		return false;
	}
		
	/*
	protected InventoryGrid getSelectedInventoryGrid() {



	}

	protected InventoryGrid getNextInventoryGrid(InventoryGrid grid, Direction direction) {



	}*/

	protected ItemInGrid getNextItemInGrid(ItemInGrid selectedItem, Direction direction) {

		int minXSelected = selectedItem.getPosXInBlocks();
		int minYSelected = selectedItem.getPosYInBlocks();
		int maxXSelected = minXSelected + selectedItem.getItemPattern().widthInBlocks - 1;
		int maxYSelected = minYSelected + selectedItem.getItemPattern().heightInBlocks - 1;

		float maxScore = 1;
		ItemInGrid currentItem = selectedItem;

		foreach(ItemInGrid item in GameManager.Instance.getInventory().getItems()) {

			if(selectedItem == item) {
				continue;
			}

			if(!item.getGridName().Equals(selectedItem.getGridName())) {
				//TODO manage
				continue;
			}

			int minX = item.getPosXInBlocks();
			int minY = item.getPosYInBlocks();
			int maxX = minX + item.getItemPattern().widthInBlocks - 1;
			int maxY = minY + item.getItemPattern().heightInBlocks - 1;

			for(int y = 0 ; y < item.getItemPattern().heightInBlocks ; y++) {
				for(int x = 0 ; x < item.getItemPattern().widthInBlocks ; x++) {

					int currentX = minX + x;
					int currentY = minY + y;

					float xScore = 0;
					float yScore = 0;

					if(direction == Direction.LEFT) {

						if(minXSelected > currentX) {

							xScore = 2 / (float)(minXSelected - currentX);

							if(currentY < minYSelected) {
								yScore = 1 / (float)(1 + minYSelected - currentY);
							} else if(currentY > maxYSelected) {
								yScore = 1 / (float)(1 + currentY - maxYSelected);
							} else {
								yScore = 1;
							}
						}

					} else if(direction == Direction.DOWN) {

						if(currentY > maxYSelected) {
							
							yScore = 2 / (float)(currentY - maxYSelected);

							if(currentX < minXSelected) {
								xScore = 1 / (float)(1 + minXSelected - currentX);
							} else if(currentX > maxXSelected) {
								xScore = 1 / (float)(1 + currentX - maxXSelected);
							} else {
								xScore = 1;
							}

						}

					} else if(direction == Direction.RIGHT) {

						if(currentX > maxXSelected) {
							
							xScore = 2 / (float)(currentX - maxXSelected);

							if(currentY < minYSelected) {
								yScore = 1 / (float)(1 + minYSelected - currentY);
							} else if(currentY > maxYSelected) {
								yScore = 1 / (float)(1 + currentY - maxYSelected);
							} else {
								yScore = 1;
							}
						}

					} else { // Direction.UP

						if(minYSelected > currentY) {
							
							yScore = 2 / (float)(minYSelected - currentY);

							if(currentX < minXSelected) {
								xScore = 1 / (float)(1 + minXSelected - currentX);
							} else if(currentX > maxXSelected) {
								xScore = 1 / (float)(1 + currentX - maxXSelected);
							} else {
								xScore = 1;
							}
						}

					}

					float score = xScore + yScore;
					if(score > maxScore) {
						maxScore = score;
						currentItem = item;
					}

				}
			}

		}

		return currentItem;
	}

}

