using System;
using UnityEngine;

public class ItemInGrid {
	
	private ItemPattern itemPattern;

	private string gridName = "";
	private int posXInBlocks = 0;
	private int posYInBlocks = 0;
	
	private int nbGrouped = 1;

	public void init(ItemPattern item, int nbGrouped) {
		init(item, null, 0, 0, nbGrouped);
	}

	public void init(ItemPattern item, string gridName, int posXInBlocks, int posYInBlocks) {
		init(item, gridName, posXInBlocks, posYInBlocks, 1);
	}

	public void init(ItemPattern itemPattern, string gridName, int posXInBlocks, int posYInBlocks, int nbGrouped) {

		if(itemPattern == null) {
			throw new System.ArgumentException();
		}
		if(nbGrouped < 0) {
			throw new System.ArgumentException();
		}

		this.itemPattern = itemPattern;
		this.gridName = gridName;

		this.posXInBlocks = posXInBlocks;
		this.posYInBlocks = posYInBlocks;

		this.nbGrouped = nbGrouped;

	}	

	public ItemPattern getItem() {
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

	public bool isFittingInsideGrid(InventoryGrid grid) {
		return (posXInBlocks >= 0 && posXInBlocks + itemPattern.widthInBlocks <= grid.maxWidth &&
			posYInBlocks >= 0 && posYInBlocks + itemPattern.heightInBlocks <= grid.maxHeight);
	}

	public void move(string gridGameObjectName, int posXInBlocks, int posYInBlocks) {

		if(string.IsNullOrEmpty(gridGameObjectName)) {
			throw new System.ArgumentException();
		}

		this.gridName = gridGameObjectName;
		this.posXInBlocks = posXInBlocks;
		this.posYInBlocks = posYInBlocks;
	}

	
	public int getNbGrouped() {
		return nbGrouped;
	}

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
	}
	
	/**
	 * Group with another item, return true if the grouping succeeded,
	 * the grouped item result is this item, the other has nbGrouped 
	 * at 0 or the remaining items number if the max has been reached.
	 * If the grouping failed, both items remains unchanged.
	 */
	public bool group(ItemInGrid other) {
		
		if(itemPattern.trKey.Equals(other.itemPattern.trKey)) {
			return false;
		}
		
		int maxResult = nbGrouped + other.nbGrouped;
		int diff = maxResult - itemPattern.maxGroupable;
		if(diff < 0) {
			diff = 0;
		}
		
		this.nbGrouped = maxResult - diff;
		other.nbGrouped = diff;
		
		return true;
	}

}
