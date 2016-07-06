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

	public CharacterBehavior characterBehavior { get; private set; }

	public string currentSpritePrefix { get; private set; }

	private Coroutine coroutineUpdateSprite;


	protected void init(Character character, CharacterBehavior characterBehavior) {

		if(characterBehavior == null) {
			throw new ArgumentException();
		}

		this.characterBehavior = characterBehavior;

		if(character is Player) {
			currentSpritePrefix = "Character.Rat.";
		} else {
			//TODO get with character type
			currentSpritePrefix = "Enemy.Insect.";
		}

		base.init(character);

	}


	protected override void FixedUpdate() {

		base.FixedUpdate();

		if(!isActiveAndEnabled) {
			return;
		}

		if(character == null) {
			//not prepared
			return;
		}

		string sortingLayerName = character.isDead() ? Constants.SORTING_LAYER_NAME_OBJECTS : Constants.SORTING_LAYER_NAME_CHARACTERS;
		GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;


		transform.position = snapToGrid(characterBehavior.transform.position);

		//Debug.Log(">>> " + transform.position.x + " - " + transform.position.y);

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
	
	public void animate(BaseCharacterState characterState, CharacterAction characterAction) {
		
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

		if(coroutineUpdateSprite != null) {
			StopCoroutine(coroutineUpdateSprite);
		}

		coroutineUpdateSprite = StartCoroutine(updateSprite(characterAction, characterAnimation));
	}

	private IEnumerator updateSprite(CharacterAction characterAction, CharacterAnimation characterAnimation) {

		ReadOnlyCollection<CharacterAnimationKey> sortedKeys = characterAnimation.sortedKeys;

		CharacterAnimationKey lastKey = sortedKeys[0];
		
		setCurrentSprite(characterAnimation, lastKey);

		float totalTime = characterAction.durationSec;
		float elapsedTime = 0;

		for(int i=1;i<sortedKeys.Count;i++) {

			CharacterAnimationKey currentKey = sortedKeys[i];

			float currentTime = currentKey.percentage * totalTime;
			
			yield return new WaitForSeconds(currentTime - elapsedTime);
			
			setCurrentSprite(characterAnimation, currentKey);

			lastKey = currentKey;
			elapsedTime = currentTime;
		}

	}

	private void setCurrentSprite(CharacterAnimation characterAnimation, CharacterAnimationKey key) {
		
		Sprite sprite = GameHelper.Instance.loadMultiSpriteAsset(
			Constants.PATH_RES_CHARACTERS + characterAnimation.textureName,
			key.imagePos + "." + characterBehavior.currentDirection.ToString());
		
		GetComponent<SpriteRenderer>().sprite = sprite;

	}

	protected abstract CharacterAnimation getCurrentCharacterAnimation(BaseCharacterState characterState);

}
