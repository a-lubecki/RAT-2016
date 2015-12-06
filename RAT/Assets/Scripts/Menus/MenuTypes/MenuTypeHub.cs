using System;

public class MenuTypeHub : AbstractMenuType {

	private Hub hub;

	public MenuTypeHub(Hub hub) : base(new AbstractSubMenuType[] { 
		new SubMenuTypeLevelUpgrade(),
		new SubMenuTypeMerchant(),  
		new SubMenuTypeItemsChest(), 
		new SubMenuTypeTeleport() }) {
		
		if(hub == null) {
			throw new System.ArgumentException();
		}

		this.hub = hub;

	}
	
}

