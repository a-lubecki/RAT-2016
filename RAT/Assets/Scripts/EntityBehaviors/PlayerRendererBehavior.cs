using UnityEngine;
using System;
using System.Collections;
using System.Collections.ObjectModel;

public class PlayerRendererBehavior : CharacterRendererBehavior {
	
	private static readonly int MAX_PLAYER_VALUE_FOR_BARS = 1500;


	public Player player {
		get {
			return character as Player;
		}
	}

	private HUDBar healthBar;
	private HUDBar staminaBar;


	public void init(Player player, PlayerBehavior playerBehavior) {

		base.init(player, playerBehavior);

	}

	protected void onBehaviorAttached() {

		healthBar = GameHelper.Instance.getHUDHealthBar().GetComponent<HUDBar>();
		staminaBar = GameHelper.Instance.getHUDStaminaBar().GetComponent<HUDBar>();

	}

	protected void updateBehavior() {
		
		base.updateBehavior();

		//update health bar
		healthBar.setBarSize(player.maxLife / (float) MAX_PLAYER_VALUE_FOR_BARS);
		healthBar.setValues(player.life, player.maxLife);

		//update stamina bar
		staminaBar.setBarSize(player.maxStamina / (float) MAX_PLAYER_VALUE_FOR_BARS);
		staminaBar.setValues(player.stamina, player.maxStamina);

	}

	protected override CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState) {

		if(characterState == BaseCharacterState.WALK) {
			return new CharacterAnimation(
				currentSpritePrefix + "Walk",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}
		
		if(characterState == BaseCharacterState.RUN) {
			return new CharacterAnimation(
				currentSpritePrefix + "Walk",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}
		
		if(characterState == PlayerState.DASH) {
			return new CharacterAnimation(
				currentSpritePrefix + "Walk",
				new CharacterAnimationKey(0, 1));
		}
		
		if(characterState == PlayerState.SHORT_ATTACK) {
			return new CharacterAnimation(
				currentSpritePrefix + "Attack",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.2f, 1));
		}
		
		if(characterState == PlayerState.HEAVY_ATTACK) {
			return new CharacterAnimation(
				currentSpritePrefix + "Attack",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}

		if(characterState == PlayerState.DEFEND) {
			return new CharacterAnimation(
				currentSpritePrefix + "Wait",
				new CharacterAnimationKey(0, 0),
				new CharacterAnimationKey(0.5f, 1));
		}

		//wait
		return new CharacterAnimation(
			currentSpritePrefix + "Wait",
			new CharacterAnimationKey(0, 0),
			new CharacterAnimationKey(0.7f, 1));
	}

}
