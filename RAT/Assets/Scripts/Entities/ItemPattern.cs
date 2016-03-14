using System;

public class ItemPattern : Displayable {

	public readonly string id;
	public readonly string imageName;

	public readonly ItemType itemType;
	public readonly ItemSubType itemSubType;
	
	public readonly int widthInBlocks;
	public readonly int heightInBlocks;

	public readonly bool isCastable;
	
	public readonly int maxGroupable;
	
	public readonly ItemPattern ammoPattern;

	
	public ItemPattern(string id, string imageKey, ItemType itemType, ItemSubType itemSubType,
		int widthInBlocks, int heightInBlocks, bool isCastable, ItemPattern ammoPattern) : this(
			id, imageKey, itemType, itemSubType, widthInBlocks, heightInBlocks, isCastable, ammoPattern, 1) {

	}

	public ItemPattern(string id, string imageKey, ItemType itemType, ItemSubType itemSubType,
		int widthInBlocks, int heightInBlocks, bool isCastable, ItemPattern ammoPattern, 
		int maxGroupable) : base("Item." + id) {

		if(string.IsNullOrEmpty(id)) {
			throw new System.ArgumentException();
		}
		if(string.IsNullOrEmpty(imageKey)) {
			throw new System.ArgumentException();
		}
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

		this.id = id;
		this.imageName = "Item." + imageKey;

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


	public string getFirstGridName() {

		if(itemType == ItemType.SPECIAL) {
			return Constants.GAME_OBJECT_NAME_GRID_SPECIAL;
		}

		return Constants.GAME_OBJECT_NAME_GRID_BAG;
	}

}

