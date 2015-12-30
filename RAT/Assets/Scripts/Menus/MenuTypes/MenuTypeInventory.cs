using UnityEngine;
using System;

public class MenuTypeInventory : AbstractMenuType {
	
	
	public MenuTypeInventory() : base(new AbstractSubMenuType[] { 
		new SubMenuTypeInventoryManagement(),
		new SubMenuTypeStoryItemsList(),
		new SubMenuTypeGameOptions(),
		new SubMenuTypeCharacterStatus() }, new Color(0.25f, 0, 0)) {
		
	}
	
}

