using System;

public class SubMenuTypeStoryItemsList : AbstractSubMenuType {

	public SubMenuTypeStoryItemsList() : base("StoryItemsList") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_STORY_ITEMS_LIST;
	}

}

