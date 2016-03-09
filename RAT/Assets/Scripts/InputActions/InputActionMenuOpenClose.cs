using System;
using UnityEngine;

public class InputActionMenuOpenClose : AbstractInputAction {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Tab
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] {
			"Start", 
			"Select",
			"TouchPadTap"//PS4
		};
	}
	
	public override bool execute() {

		Player player = GameHelper.Instance.getPlayer();
		if(!player.isControlsEnabled || !player.isControlsEnabledWhileAnimating) {
			return false;
		}

		Menu menu = GameHelper.Instance.getMenu();

		if(menu.isOpened()) {
			menu.closeAny();
		} else {
			menu.open(Constants.MENU_TYPE_INVENTORY);
		}

		return true;
	}

}

