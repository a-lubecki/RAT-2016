using System;

public class Item : Displayable {

	public readonly ItemType itemType;
	public readonly ItemSubType itemSubType;
	
	public readonly int widthInBlocks;
	public readonly int heightInBlocks;

	public readonly bool isCastable;
	
	public readonly int maxGroupable;
	private int nbGrouped = 1;
	
	public readonly AmmoType ammoType;

	
	
	public Item(string trKey, ItemType itemType, ItemSubType itemSubType,
	            int widthInBlocks, int heightInBlocks, bool isCastable) : this(trKey, itemType, itemSubType,
	                                                               widthInBlocks, heightInBlocks, isCastable,
	                                                               1, null) {

	}

	public Item(string trKey, ItemType itemType, ItemSubType itemSubType,
	            int widthInBlocks, int heightInBlocks, bool isCastable,
	             int maxGroupable, AmmoType ammoType) : base("Item." + trKey) {

		if(itemType == null) {
			throw new System.ArgumentException();
		}
		if(itemSubType == null) {
			throw new System.ArgumentException();
		}

		if(widthInBlocks <= 0) {
			throw new System.ArgumentException();
		}
		if(heightInBlocks <= 0) {
			throw new System.ArgumentException();
		}
		if(maxGroupable <= 0) {
			throw new System.ArgumentException();
		}

		this.itemType = itemType;
		this.itemSubType = itemSubType;

		this.widthInBlocks = widthInBlocks;
		this.heightInBlocks = heightInBlocks;

		this.isCastable = isCastable;

		this.maxGroupable = maxGroupable;

		this.ammoType = ammoType;//can be null

	}

	private string getDescription() {
		return Constants.tr(trKey + ".Description");
	}
	
	public bool isValid() {
		return (nbGrouped > 0);
	}
	
	public int getNbGrouped() {
		return nbGrouped;
	}

	public void setNbGrouped(int nbGrouped) {
		
		if(nbGrouped <= 0) {
			throw new System.ArgumentException();
		}
		if(nbGrouped > maxGroupable) {
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
	public bool group(Item other) {

		if(trKey.Equals(other.trKey)) {
			return false;
		}

		int maxResult = nbGrouped + other.nbGrouped;
		int diff = maxResult - maxGroupable;
		if(diff < 0) {
			diff = 0;
		}
		
		this.nbGrouped = maxResult - diff;
		other.nbGrouped = diff;
		
		return true;
	}

}

