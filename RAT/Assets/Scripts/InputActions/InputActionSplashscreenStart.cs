using System;
using UnityEngine;

public class InputActionSplashscreenStart : AbstractInputAction {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Return,
			KeyCode.KeypadEnter,
			KeyCode.Space
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] {
			"Action1",
			"Action2",
			"Start", 
			"Select",
			"TouchPadTap"//PS4
		};
	}
	
	public override bool execute() {

		GameHelper.Instance.getSplashScreenManager().hideSplashScreenTitle();
	
		return true;
	}

}

