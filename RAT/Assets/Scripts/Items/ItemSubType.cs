using System;

public class ItemSubType : Displayable {
	
	public static readonly ItemSubType WEAPON_CLAWS = new ItemSubType("WEAPON_CLAWS");
	public static readonly ItemSubType WEAPON_OFFENSIVE = new ItemSubType("WEAPON_OFFENSIVE");
	public static readonly ItemSubType WEAPON_DEFENSIVE = new ItemSubType("WEAPON_DEFENSIVE");
	
	public static readonly ItemSubType EQUIPMENT_HEAD = new ItemSubType("EQUIPMENT_HEAD");
	public static readonly ItemSubType EQUIPMENT_BODY = new ItemSubType("EQUIPMENT_BODY");
	public static readonly ItemSubType EQUIPMENT_ARMS = new ItemSubType("EQUIPMENT_ARMS");
	public static readonly ItemSubType EQUIPMENT_LEGS = new ItemSubType("EQUIPMENT_LEGS");

	public static readonly ItemSubType OBJECT_OFFENSIVE = new ItemSubType("OBJECT_OFFENSIVE");
	public static readonly ItemSubType OBJECT_BOOST = new ItemSubType("OBJECT_BOOST");
	public static readonly ItemSubType OBJECT_AMMO = new ItemSubType("OBJECT_AMMO");
	
	public static readonly ItemSubType HEAL_INSTANT = new ItemSubType("HEAL_INSTANT");
	public static readonly ItemSubType HEAL_PERSISTENT = new ItemSubType("HEAL_PERSISTENT");
	
	public static readonly ItemSubType SPECIAL_DATA = new ItemSubType("SPECIAL_DATA");
	public static readonly ItemSubType SPECIAL_KEY = new ItemSubType("SPECIAL_KEY");


	public ItemSubType(string key) : base("ItemSubType." + key) {
		
	}

}

