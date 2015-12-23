using System;

public class SubMenuTypeMerchant : AbstractSubMenuType {

	public SubMenuTypeMerchant() : base("Merchant") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_MERCHANT;
	}

}

