using System;

public class ItemPattern : Displayable {

	public readonly string id;

	public readonly ItemType itemType;
	public readonly ItemSubType itemSubType;
	
	public readonly int widthInBlocks;
	public readonly int heightInBlocks;

	public readonly bool isCastable;
	
	public readonly int maxGroupable;
	
	public readonly ItemPattern ammoPattern;

	//TODO sprite name
	
	
	public ItemPattern(string id, string trKey, ItemType itemType, ItemSubType itemSubType,
		int widthInBlocks, int heightInBlocks, bool isCastable, ItemPattern ammoPattern) : this(
			id, trKey, itemType, itemSubType, widthInBlocks, heightInBlocks, isCastable, ammoPattern, 1) {

	}

	public ItemPattern(string id, string trKey, ItemType itemType, ItemSubType itemSubType,
		int widthInBlocks, int heightInBlocks, bool isCastable, ItemPattern ammoPattern, 
		int maxGroupable) : base("Item." + trKey) {

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

		this.ammoPattern = ammoPattern;//can be null

	}

	private string getDescription() {
		return Constants.tr(trKey + ".Description");
	}

}

