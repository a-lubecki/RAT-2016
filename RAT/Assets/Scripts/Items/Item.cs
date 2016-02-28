using System;

public class Item : Displayable {

	public readonly string id;

	public readonly ItemType itemType;
	public readonly ItemSubType itemSubType;
	
	public readonly int widthInBlocks;
	public readonly int heightInBlocks;

	public readonly bool isCastable;
	
	public readonly int maxGroupable;
	
	public readonly AmmoType ammoType;

	//TODO sprite name
	
	
	public Item(string id, string trKey, ItemType itemType, ItemSubType itemSubType,
	            int widthInBlocks, int heightInBlocks, bool isCastable) : this(id, trKey, itemType, itemSubType,
	                                                               widthInBlocks, heightInBlocks, isCastable,
	                                                               1, null) {

	}

	public Item(string id, string trKey, ItemType itemType, ItemSubType itemSubType,
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

}

