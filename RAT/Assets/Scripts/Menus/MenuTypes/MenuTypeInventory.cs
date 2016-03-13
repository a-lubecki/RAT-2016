using UnityEngine;
using System;

public class MenuTypeInventory : AbstractMenuType {
	
	
	public MenuTypeInventory() : base(new AbstractSubMenuType[] { 
		Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT,
		new SubMenuTypeStoryItemsList(),
		new SubMenuTypeGameOptions(),
		new SubMenuTypeCharacterStatus() }, new Color(0.25f, 0, 0)) {
		
	}
	
}

