using System;

public class ItemSubType : Displayable {
	
	public static readonly ItemSubType OBJECT_AMMO = new ItemSubType("OBJECT_AMMO");
	public static readonly ItemSubType OBJECT_OFFENSIVE = new ItemSubType("OBJECT_OFFENSIVE");
	public static readonly ItemSubType OBJECT_BOOST = new ItemSubType("OBJECT_BOOST");
	
	public static readonly ItemSubType HEAL_INSTANT = new ItemSubType("HEAL_INSTANT");
	public static readonly ItemSubType HEAL_PERSISTENT = new ItemSubType("HEAL_PERSISTENT");

	public static readonly ItemSubType WEAPON_CLAWS = new ItemSubType("WEAPON_CLAWS");
	public static readonly ItemSubType WEAPON_OFFENSIVE = new ItemSubType("WEAPON_OFFENSIVE");
	public static readonly ItemSubType WEAPON_DEFENSIVE = new ItemSubType("WEAPON_DEFENSIVE");

	public static readonly ItemSubType EQUIPMENT_HEAD = new ItemSubType("EQUIPMENT_HEAD");
	public static readonly ItemSubType EQUIPMENT_BODY = new ItemSubType("EQUIPMENT_BODY");
	public static readonly ItemSubType EQUIPMENT_ARMS = new ItemSubType("EQUIPMENT_ARMS");
	public static readonly ItemSubType EQUIPMENT_LEGS = new ItemSubType("EQUIPMENT_LEGS");

	public static readonly ItemSubType SPECIAL_DATA = new ItemSubType("SPECIAL_DATA");
	public static readonly ItemSubType SPECIAL_KEY = new ItemSubType("SPECIAL_KEY");

	public static ItemSubType[] getValues(ItemType itemType) {

		if(itemType == null) {
			throw new ArgumentException();
		}

		if(itemType == ItemType.OBJECT) {
			return new ItemSubType[] { OBJECT_OFFENSIVE, OBJECT_BOOST, OBJECT_AMMO };
		}
		if(itemType == ItemType.HEAL) {
			return new ItemSubType[] { HEAL_INSTANT, HEAL_PERSISTENT };
		}
		if(itemType == ItemType.WEAPON) {
			return new ItemSubType[] { WEAPON_CLAWS, WEAPON_OFFENSIVE, WEAPON_DEFENSIVE };
		}
		if(itemType == ItemType.EQUIPMENT) {
			return new ItemSubType[] { EQUIPMENT_HEAD, EQUIPMENT_BODY, EQUIPMENT_ARMS, EQUIPMENT_LEGS };
		}
		if(itemType == ItemType.SPECIAL) {
			return new ItemSubType[] { SPECIAL_DATA, SPECIAL_KEY };
		}

		throw new NotSupportedException();
	}

	public static ItemSubType fromString(ItemType itemType, string key) {

		foreach(ItemSubType itemSubType in getValues(itemType)) {

			if(itemSubType.key.Equals(key)) {
				return itemSubType;
			}
		}

		throw new InvalidOperationException("The item subtype doesn't exist : " + key);
	}


	public readonly string key;

	public ItemSubType(string key) : base("ItemSubType." + key) {

		if(key == null) {
			throw new ArgumentException();
		}
		this.key = key;
	}

}

