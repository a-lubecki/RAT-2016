using System;

public class MenuTypeInventory : AbstractMenuType {
	
	
	public MenuTypeInventory() : base(new AbstractSubMenuType[] { 
		new SubMenuTypeInventoryManagment(),
		new SubMenuTypeItemsList(),
		new SubMenuTypeGameOptions(),
		new SubMenuTypeCharacterStatus() }) {
		
	}
	
}

