using UnityEngine;
using System;

public class MenuTypeHub : AbstractMenuType {

	private Hub hub;

	public MenuTypeHub(Hub hub) : base(new AbstractSubMenuType[] { 
		new SubMenuTypeLevelUpgrade(),
		new SubMenuTypeMerchant(),  
		new SubMenuTypeItemsChest(), 
		new SubMenuTypeTeleport() }, new Color(0, 0.3f, 0.4f)) {
		
		if(hub == null) {
			throw new System.ArgumentException();
		}

		this.hub = hub;

	}
	
}

