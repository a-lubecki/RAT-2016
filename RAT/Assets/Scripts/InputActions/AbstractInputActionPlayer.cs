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

		PlayerBehavior playerBehavior = player.findBehavior<PlayerBehavior>();//TODO refaire
		if (playerBehavior == null) {
			return false;
		}

		if(!playerBehavior.isControlsEnabled || !playerBehavior.isControlsEnabledWhileAnimating) {
			return false;
		}

		return true;
	}

}

