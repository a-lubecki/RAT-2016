using System;

public class SubMenuTypeTeleport : AbstractSubMenuType {

	public SubMenuTypeTeleport() : base("Teleport") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_TELEPORT;
	}

}

