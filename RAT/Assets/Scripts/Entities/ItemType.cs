using System;

public class ItemType : Displayable {

	public static readonly ItemType OBJECT = new ItemType("OBJECT");
	public static readonly ItemType HEAL = new ItemType("HEAL");
	public static readonly ItemType WEAPON = new ItemType("WEAPON");
	public static readonly ItemType EQUIPMENT = new ItemType("EQUIPMENT");
	public static readonly ItemType SPECIAL = new ItemType("SPECIAL");

	public static ItemType[] getValues() {
		return new ItemType[] { OBJECT, HEAL, WEAPON, EQUIPMENT, SPECIAL };
	}

	public static ItemType fromString(string key) {

		foreach(ItemType itemType in getValues()) {

			if(itemType.key.Equals(key)) {
				return itemType;
			}
		}

		throw new InvalidOperationException("The item type doesn't exist : " + key);
	}


	public readonly string key;

	public ItemType(string key) : base("ItemType." + key) {
	
		if(key == null) {
			throw new ArgumentException();
		}
		this.key = key;
	}

}

