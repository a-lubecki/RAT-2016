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
		
		PlayerBehavior playerBehavior = GameHelper.Instance.findPlayerBehavior();
		if(!playerBehavior.isControlsEnabled || !playerBehavior.isControlsEnabledWhileAnimating) {
			return false;
		}

		return true;
	}

}

