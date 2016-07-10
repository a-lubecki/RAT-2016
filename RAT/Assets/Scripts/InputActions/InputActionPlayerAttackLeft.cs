using System;
using UnityEngine;

public class InputActionPlayerAttackLeft : AbstractInputActionPlayer {
	
	public override KeyCode[] getDefaultActionKeys() {
		return new KeyCode[] { 
			KeyCode.Keypad1,
			KeyCode.O
		};
	}

	public override string[] getDefaultActionButtons() {
		return new string[] { 
			"LeftBumper",
			"LeftTrigger"
		};
	}
	
	public override bool execute() {
		
		if(!base.execute()) {
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

		playerBehavior.tryLeftAttack();

		return true;
	}

}

