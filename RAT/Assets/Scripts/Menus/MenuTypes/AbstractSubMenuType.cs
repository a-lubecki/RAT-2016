using System;
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


	private int selectionLevel = 0;
	
	public int getSelectionLevel() {
		//it's 0 by default, if an item is selected the level is 1, if a subitem is selected the level is 2, etc
		return selectionLevel;
	}

	protected void incrementSelectionLevel() {
		selectionLevel++;
	}
	protected void decrementSelectionLevel() {
		selectionLevel--;
	}

	public virtual void validate() {

		if(selectionLevel == 0) {

			incrementSelectionLevel();

			//TODO select
		
		} else {


		}

	}
	
	public virtual void cancel() {

		if(selectionLevel == 1) {
			
			decrementSelectionLevel();
			
			//TODO deselect
			
		} else {

		}

	}
	
	public virtual void navigateUp() {
	}
	
	public virtual void navigateDown() {
	}
	
	public virtual void navigateRight() {
	}
	
	public virtual void navigateLeft() {
	}

}

