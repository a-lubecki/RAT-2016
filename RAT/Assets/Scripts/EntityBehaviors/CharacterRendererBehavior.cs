using UnityEngine;
using System;
using System.Collections;
using System.Collections.ObjectModel;

public abstract class CharacterRendererBehavior : BaseEntityBehavior {

	public Character character {
		get {
			return (Character) entity;
		}
	}

	public string currentSpritePrefix { get; private set; }

	private Coroutine coroutineUpdateSprite;

	//used to update sprite only if it needs to changed
	CharacterAnimation previousCharacterAnimation;
	CharacterAnimationKey previousCharacterAnimationKey;
	CharacterDirection previousDirection;

	protected void init(Character character) {

		if(character is Player) {
			currentSpritePrefix = "Character.Rat.";
		} else {
			//TODO get with character type
			currentSpritePrefix = "Enemy.Insect.";
		}

		base.init(character);

	}

	protected override void updateBehavior() {

		//display the right sprite
		CharacterAnimation characterAnimation = getCurrentCharacterAnimation(character.currentState);

		if (characterAnimation != null) {

			int frame = character.animationPercentage / (float)characterAnimation.sortedKeys.Count;
			CharacterAnimationKey characterAnimationKey = characterAnimation.sortedKeys[frame];

			CharacterDirection currentDirection = getCharacterDirection(previousDirection, 55);

			//change sprite if something changed from the last time
			if (characterAnimation != previousCharacterAnimation ||
				characterAnimationKey != previousCharacterAnimationKey ||
				currentDirection != previousDirection) {

				Sprite sprite = GameHelper.Instance.loadMultiSpriteAsset(
					Constants.PATH_RES_CHARACTERS + characterAnimation.textureName,
					characterAnimationKey.imagePos + "." + currentDirection.ToString());

				GetComponent<SpriteRenderer>().sprite = sprite;

				previousCharacterAnimation = characterAnimation;
				previousCharacterAnimationKey = characterAnimationKey;
				previousDirection = currentDirection;
			}
		}


		//change the layer if dead
		string sortingLayerName = character.isDead() ? Constants.SORTING_LAYER_NAME_OBJECTS : Constants.SORTING_LAYER_NAME_CHARACTERS;
		GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;

		//move to the right position
		transform.position = snapToGrid(new Vector2(character.realPosX, character.realPosY));


		//TODO update "ground" tiles around player with GameObjects pooling : http://blogs.msdn.com/b/dave_crooks_dev_blog/archive/2014/07/21/object-pooling-for-unity3d.aspx

	}


	private static Vector2 snapToGrid(Vector2 vector) {
		return new Vector2(
			snapToGrid(vector.x + Constants.PIXEL_SIZE * 0.5f), 
			snapToGrid(vector.y - Constants.PIXEL_SIZE * 0.5f));
	}
	
	private static float snapToGrid(float value) {

		float diff = value % Constants.PIXEL_SIZE; // for PIXEL_SIZE == 1, diff : 385.7 % 1 = 0.7
		return value - diff; // 385.7 - 0.7 = 385.7
	}

	protected abstract CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState);


	public CharacterDirection getCharacterDirection(CharacterDirection currentDirection, int halfAngle) {

		if(currentDirection == CharacterDirection.RIGHT ||
			currentDirection == CharacterDirection.LEFT) {

			if(isCharacterDirectionRight(character.angleDegrees, halfAngle)) {
				return CharacterDirection.RIGHT;
			} 
			if(isCharacterDirectionLeft(character.angleDegrees, halfAngle)) {
				return CharacterDirection.LEFT;
			} 
			if(isCharacterDirectionUp(character.angleDegrees, halfAngle)) {
				return CharacterDirection.UP;
			} 
			if(isCharacterDirectionDown(character.angleDegrees, halfAngle)) {
				return CharacterDirection.DOWN;
			}
		}

		if(currentDirection == CharacterDirection.UP ||
			currentDirection == CharacterDirection.DOWN) {

			if(isCharacterDirectionUp(character.angleDegrees, halfAngle)) {
				return CharacterDirection.UP;
			} 
			if(isCharacterDirectionDown(character.angleDegrees, halfAngle)) {
				return CharacterDirection.DOWN;
			} 
			if(isCharacterDirectionRight(character.angleDegrees, halfAngle)) {
				return CharacterDirection.RIGHT;
			} 
			if(isCharacterDirectionLeft(character.angleDegrees, halfAngle)) {
				return CharacterDirection.LEFT;
			}
		}

		return CharacterDirection.DOWN;
	}

	private static bool isCharacterDirectionUp(float angle, int halfAngle) {
		return (360 - halfAngle <= angle || angle <= halfAngle);
	}
	private static bool isCharacterDirectionRight(float angle, int halfAngle) {
		return (90 - halfAngle <= angle && angle <= 90 + halfAngle);
	}
	private static bool isCharacterDirectionDown(float angle, int halfAngle) {
		return (180 - halfAngle <= angle && angle <= 180 + halfAngle);
	}
	private static bool isCharacterDirectionLeft(float angle, int halfAngle) {
		return (270 - halfAngle <= angle && angle <= 270 + halfAngle);
	}

	/*
	private void animate(BaseCharacterState characterState, CharacterAction characterAction) {
		
		if(characterAction == null) {
			return;
		}

		if(!isActiveAndEnabled) {
			return;
		}

		CharacterAnimation characterAnimation = getCurrentCharacterAnimation(characterState);
		if(characterAnimation == null) {
			return;
		}

		updateSprite(characterAction, characterAnimation);
	}

	private IEnumerator updateSprite(CharacterAction characterAction, CharacterAnimation characterAnimation) {

		ReadOnlyCollection<CharacterAnimationKey> sortedKeys = characterAnimation.sortedKeys;

		CharacterAnimationKey lastKey = sortedKeys[0];
		
		setCurrentSprite(characterAnimation, lastKey);

		float totalTime = characterAction.durationSec;
		float elapsedTime = 0;

		for (int i = 1 ; i < sortedKeys.Count ; i++) {

			CharacterAnimationKey currentKey = sortedKeys[i];

			float currentTime = currentKey.percentage * totalTime;
			
			yield return new WaitForSeconds(currentTime - elapsedTime);
			
			setCurrentSprite(characterAnimation, currentKey);

			lastKey = currentKey;
			elapsedTime = currentTime;
		}

	}*/

}
