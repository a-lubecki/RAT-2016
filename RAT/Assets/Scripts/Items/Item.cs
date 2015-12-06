using System;

public class Item : Displayable {

	
	private readonly string keyName;

	public readonly ItemType itemType;
	public readonly ItemSubType itemSubType;
	
	public readonly int widthInBlocks;
	public readonly int heightInBlocks;

	public bool isCastable;

	public readonly int maxDockable;
	
	public readonly AmmoType ammoType;

	
	public Item(string trKey, ItemType itemType, ItemSubType itemSubType) : base("Item." + trKey) {

		if(itemType == null) {
			throw new System.ArgumentException();
		}
		if(itemSubType == null) {
			throw new System.ArgumentException();
		}

		this.keyName = "Item." + trKey;
		this.itemType = itemType;
		this.itemSubType = itemSubType;

	}

	private string getDescription() {
		return Constants.tr(trKey + ".Description");
	}


}

