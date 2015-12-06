using System;

public class MenuTypeInventory : AbstractMenuType {
	
	
	public MenuTypeInventory() : base(new AbstractSubMenuType[] { 
		new SubMenuTypeInventoryManagment(),
		new SubMenuTypeStoryItemsList(),
		new SubMenuTypeGameOptions(),
		new SubMenuTypeCharacterStatus() }) {
		
	}
	
}

