using System;
using UnityEngine;

public class ItemInGrid : MonoBehaviour {
	
	private Item item;

	private string gridGameObjectName = "";
	private int posXInBlocks = 0;
	private int posYInBlocks = 0;
	
	private int nbGrouped = 1;


	public void init(Item item, string gridGameObjectName, int posXInBlocks, int posYInBlocks) {
		init(item, gridGameObjectName, posXInBlocks, posYInBlocks, 1);
	}

	public void init(Item item, string gridGameObjectName, int posXInBlocks, int posYInBlocks, int nbGrouped) {

		if(item == null) {
			throw new System.ArgumentException();
		}
		if(string.IsNullOrEmpty(gridGameObjectName)) {
			throw new System.ArgumentException();
		}
		if(nbGrouped < 0) {
			throw new System.ArgumentException();
		}

		this.item = item;
		this.gridGameObjectName = gridGameObjectName;

		this.posXInBlocks = posXInBlocks;
		this.posYInBlocks = posYInBlocks;

		this.nbGrouped = nbGrouped;

	}	

	public Item getItem() {
		return item;
	}

	public string getGridGameObjectName() {
		return gridGameObjectName;
	}

	public int getPosXInBlocks() {
		return posXInBlocks;	
	}
	public int getPosYInBlocks() {
		return posYInBlocks;
	}

	public bool isFittingInsideGrid(InventoryGrid grid) {
		return (posXInBlocks >= 0 && posXInBlocks + item.widthInBlocks <= grid.maxWidth &&
		        posYInBlocks >= 0 && posYInBlocks + item.heightInBlocks <= grid.maxHeight);
	}

	public void move(string gridGameObjectName, int posXInBlocks, int posYInBlocks) {

		if(string.IsNullOrEmpty(gridGameObjectName)) {
			throw new System.ArgumentException();
		}

		this.gridGameObjectName = gridGameObjectName;
		this.posXInBlocks = posXInBlocks;
		this.posYInBlocks = posYInBlocks;
	}

	
	public int getNbGrouped() {
		return nbGrouped;
	}

	public bool isGroupValid() {
		return (nbGrouped > 0 && nbGrouped <= item.maxGroupable);
	}

	public void setNbGrouped(int nbGrouped) {
		
		if(nbGrouped <= 0) {
			throw new System.ArgumentException();
		}
		if(nbGrouped > item.maxGroupable) {
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
		
		if(item.trKey.Equals(other.item.trKey)) {
			return false;
		}
		
		int maxResult = nbGrouped + other.nbGrouped;
		int diff = maxResult - item.maxGroupable;
		if(diff < 0) {
			diff = 0;
		}
		
		this.nbGrouped = maxResult - diff;
		other.nbGrouped = diff;
		
		return true;
	}

}
