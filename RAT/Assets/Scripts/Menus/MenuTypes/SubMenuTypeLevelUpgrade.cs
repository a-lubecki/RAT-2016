using System;

public class SubMenuTypeLevelUpgrade : AbstractSubMenuType {

	public SubMenuTypeLevelUpgrade() : base("LevelUpgrade") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_LEVEL_UPGRADE;
	}

}

