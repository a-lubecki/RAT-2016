using System;
using UnityEngine;

public abstract class AbstractInputActionPlayer : AbstractInputAction {

	public override bool execute() {
		
		Menu menu = GameHelper.Instance.getMenu();
		if(menu.isAnimating()) {
			return false;
		}
		if(menu.isOpened()) {
			return false;
		}
		
		Player player = GameHelper.Instance.getPlayer();
		if (player == null) {
			return false;
		}

		if(!player.isControlsEnabled || !player.isControlsEnabledWhileAnimating) {
			return false;
		}

		return true;
	}

}

