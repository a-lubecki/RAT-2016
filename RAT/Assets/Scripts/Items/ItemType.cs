using System;

public class ItemType : Displayable {

	public static readonly ItemType WEAPON = new ItemType("WEAPON");
	public static readonly ItemType EQUIPMENT = new ItemType("EQUIPMENT");
	public static readonly ItemType OBJECT = new ItemType("OBJECT");
	public static readonly ItemType HEAL = new ItemType("HEAL");
	
	public ItemType(string key) : base("ItemType." + key) {
		
	}

}

