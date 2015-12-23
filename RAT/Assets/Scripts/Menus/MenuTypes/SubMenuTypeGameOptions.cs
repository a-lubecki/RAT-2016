using System;

public class SubMenuTypeGameOptions : AbstractSubMenuType {

	public SubMenuTypeGameOptions() : base("GameOptions") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_GAME_OPTIONS;
	}

}

