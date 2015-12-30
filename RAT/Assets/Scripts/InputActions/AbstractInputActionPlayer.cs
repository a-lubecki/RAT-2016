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
		
		PlayerCollider playerController = GameHelper.Instance.getPlayerControls();
		if(!playerController.isControlsEnabled || !playerController.isControlsEnabledWhileAnimating) {
			return false;
		}

		return true;
	}

}

