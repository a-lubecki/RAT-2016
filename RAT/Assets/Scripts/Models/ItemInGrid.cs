using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInGrid : ISelectable, IActionnable {
	
	private ItemPattern itemPattern;

	private string gridName;
	private int posXInBlocks;
	private int posYInBlocks;
	private Orientation orientation;

	private int nbGrouped;

	public ItemInGrid(ItemPattern item, string gridName)
		: this(item, gridName, 0, 0, Orientation.FACE, 1) {

	}

	public ItemInGrid(ItemPattern itemPattern, string gridName, int posXInBlocks, int posYInBlocks, Orientation orientation, int nbGrouped) {

		if(itemPattern == null) {
			throw new System.ArgumentException();
		}
		if(nbGrouped <= 0 || nbGrouped > itemPattern.maxGroupable) {
			throw new ArgumentException("Nb grouped value must be between 0 and " + itemPattern.maxGroupable + " : " + nbGrouped);
		}

		this.itemPattern = itemPattern;
		this.gridName = gridName;

		this.posXInBlocks = posXInBlocks;
		this.posYInBlocks = posYInBlocks;
		this.orientation = orientation;

		this.nbGrouped = nbGrouped;

	}	

	public ItemPattern getItemPattern() {
		return itemPattern;
	}

	public string getGridName() {
		return gridName;
	}

	public int getPosXInBlocks() {
		return posXInBlocks;	
	}
	public int getPosYInBlocks() {
		return posYInBlocks;
	}
	public Orientation getOrientation() {
		return orientation;
	}
	/*
	public bool isFittingInsideGrid(InventoryGrid grid) {
		
		if (posXInBlocks < 0) {
			return false;
		}
		if(posYInBlocks < 0) {
			return false;
		}

		if(posXInBlocks + itemPattern.widthInBlocks > grid.maxWidth && 
			posYInBlocks + itemPattern.widthInBlocks > grid.maxHeight) {
			return false;
		}

		if(posYInBlocks + itemPattern.heightInBlocks > grid.maxHeight && 
			posXInBlocks + itemPattern.heightInBlocks > grid.maxWidth) {
			return false;
		}

		return true;
	}

	public void move(string gridGameObjectName, int posXInBlocks, int posYInBlocks) {

		if(string.IsNullOrEmpty(gridGameObjectName)) {
			throw new System.ArgumentException();
		}

		this.gridName = gridGameObjectName;
		this.posXInBlocks = posXInBlocks;
		this.posYInBlocks = posYInBlocks;
	}
*/
	
	public int getNbGrouped() {
		return nbGrouped;
	}
	/*
	public bool isGroupValid() {
		return (nbGrouped > 0 && nbGrouped <= itemPattern.maxGroupable);
	}

	public void setNbGrouped(int nbGrouped) {
		
		if(nbGrouped <= 0) {
			throw new System.ArgumentException();
		}
		if(nbGrouped > itemPattern.maxGroupable) {
			throw new System.ArgumentException();
		}
		
		this.nbGrouped = nbGrouped;
	}*/
	/*
	public bool isGroupableWith(ItemInGrid other) {

		if(other == null) {
			return false;
		}
		if(other == this) {
			return false;
		}
		if(!itemPattern.trKey.Equals(other.itemPattern.trKey)) {
			return false;
		}

		return (nbGrouped < itemPattern.maxGroupable);
	}
	*/
	/**
	 * Group with another item, return the item if the grouping succeeded,
	 * the grouped item result location is the current item's.
	 * If the grouping failed, return null.
	 */
	public ItemInGrid newGroupedItem(ItemPattern otherItemPattern, int otherNbGrouped) {

		if(otherItemPattern == null) {
			return this;
		}
		if(!itemPattern.trKey.Equals(otherItemPattern.trKey)) {
			return this;
		}
		
		int maxResult = nbGrouped + otherNbGrouped;
		int diff = maxResult - itemPattern.maxGroupable;
		if(diff < 0) {
			diff = 0;
		}
		
		int res = maxResult - diff;
		if(res <= 0) {
			return null;
		}
	
		return new ItemInGrid(itemPattern, 
			gridName, 
			posXInBlocks,
			posYInBlocks, 
			orientation,
			res);
	}

	void ISelectable.onSelect() {

		GameHelper.Instance.getMenu().getCurrentSubMenuType().onItemSelected(this);
	}

	void ISelectable.onDeselect() {

		GameHelper.Instance.getMenu().getCurrentSubMenuType().onItemDeselected();
	}

	void ISelectable.onSelectionValidated() {

		//display glassview on top
		GameObject glassGameObject = GameHelper.Instance.getForegroundGlassGameObject();
		glassGameObject.transform.SetAsLastSibling();
		glassGameObject.GetComponent<Image>().enabled = true;

		GameHelper.Instance.getItemInGridBehavior(this).transform.SetParent(glassGameObject.transform);
		GameHelper.Instance.getMenuCursorBehavior().transform.SetParent(glassGameObject.transform);

		List<BaseAction> itemActions = new List<BaseAction>();
		itemActions.Add(new ActionItemInGridMove(this));
		itemActions.Add(new ActionItemInGridSendToHub(this));
		itemActions.Add(new ActionItemInGridCast(this));
		ItemInGridActionsManager.Instance.showActions(itemActions);

	}

	void ISelectable.onSelectionCancelled() {

		//hide glassview
		hideSelection();
	}

	private void hideSelection() {

		GameObject glassGameObject = GameHelper.Instance.getForegroundGlassGameObject();
		glassGameObject.GetComponent<Image>().enabled = false;

		ItemInGridBehavior itemInGridBehavior = glassGameObject.transform.GetComponentInChildren<ItemInGridBehavior>();

		InventoryGrid grid = GameHelper.Instance.getMenu().getCurrentSubMenuType().findInventoryGrid(getGridName());
		itemInGridBehavior.transform.SetParent(grid.transform);

		MenuCursorBehavior menuCursorBehavior = glassGameObject.transform.GetComponentInChildren<MenuCursorBehavior>();
		menuCursorBehavior.transform.SetParent(GameHelper.Instance.getMenu().transform);

		ItemInGridActionsManager.Instance.hideActions();
	}

	void IActionnable.notifyActionShown(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionHidden(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		hideSelection();
	}

}
