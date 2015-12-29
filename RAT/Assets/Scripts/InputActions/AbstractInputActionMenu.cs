using System;
using UnityEngine;

public abstract class AbstractInputActionMenu : AbstractInputAction {

	public override bool execute() {
		
		Menu menu = GameHelper.Instance.getMenu();
		if(menu.isAnimating()) {
			return false;
		}
		if(!menu.isOpened()) {
			return false;
		}

		return true;
	}

}

