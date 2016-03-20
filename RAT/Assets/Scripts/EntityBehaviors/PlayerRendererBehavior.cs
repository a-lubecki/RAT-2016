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
	public PlayerBehavior playerBehavior {
		get {
			return characterBehavior as PlayerBehavior;
		}
	}

	public void init(Player player, PlayerBehavior playerBehavior) {

		base.init(player, playerBehavior);

	}

	protected override void FixedUpdate() {

		if(player == null) {
			//not prepared
			return;
		}

		base.FixedUpdate();

		//update health bar
		HUDBar healthBar = GameHelper.Instance.getHUDHealthBar().GetComponent<HUDBar>();
		
		if(healthBar != null) {
			healthBar.setBarSize(player.maxLife / (float) MAX_PLAYER_VALUE_FOR_BARS);
			healthBar.setValues(player.life, player.maxLife);
		}
		
		//update stamina bar
		HUDBar staminaBar = GameHelper.Instance.getHUDStaminaBar().GetComponent<HUDBar>();
		
		if(staminaBar != null) {
			staminaBar.setBarSize(player.maxStamina / (float) MAX_PLAYER_VALUE_FOR_BARS);
			staminaBar.setValues(player.stamina, player.maxStamina);
		}

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
