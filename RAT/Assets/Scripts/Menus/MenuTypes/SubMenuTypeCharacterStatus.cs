using System;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuTypeCharacterStatus : AbstractSubMenuType {

	public SubMenuTypeCharacterStatus() : base("CharacterStatus") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_CHARACTER_STATUS;
	}

}

