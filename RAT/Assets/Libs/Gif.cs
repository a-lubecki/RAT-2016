using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gif : MonoBehaviour {

	private Coroutine coroutineAnimation;

	public float durationSec = 1;
	public bool isLoop;
	
	public Sprite[] sprites;


	private Image image;
	private SpriteRenderer spriteRenderer;


	protected void Awake() {
		
		image = GetComponent<Image>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void startAnimation() {

		stopAnimation();

		coroutineAnimation = StartCoroutine(animate());

	}

	public void stopAnimation() {
		
		if(coroutineAnimation == null) {
			return;
		}

		StopCoroutine(coroutineAnimation);

		coroutineAnimation = null;

		Sprite currentSprite = sprites[0];
		if(image != null) {
			image.sprite = currentSprite;
		}
		if(spriteRenderer != null) {
			spriteRenderer.sprite = currentSprite;
		}
	}

	private IEnumerator animate() {
		
		float durationSecTmp = durationSec;
		bool isLoopTmp = isLoop;

		if(durationSecTmp <= 0) {
			coroutineAnimation = null;
			
			Sprite currentSprite = sprites[0];
			if(image != null) {
				image.sprite = currentSprite;
			}
			if(spriteRenderer != null) {
				spriteRenderer.sprite = currentSprite;
			}
			return false;
		}

		Sprite[] spritesTmp = new Sprite[sprites.Length];

		int i = 0;
		foreach(Sprite sprite in sprites) {
			spritesTmp[i] = sprite;
			i++;
		}

		int nbImages = sprites.Length;

		float secondsToWait = durationSecTmp / (float)nbImages;

		for(i = 0 ; i < nbImages ; i++) {
			
			Sprite currentSprite = sprites[i];
			if(image != null) {
				image.sprite = currentSprite;
			}
			if(spriteRenderer != null) {
				spriteRenderer.sprite = currentSprite;
			}

			yield return new WaitForSeconds(secondsToWait);

			if(isLoopTmp && i >= nbImages-1) {
				//reset
				i = -1;
			}
		}

		coroutineAnimation = null;
	
	}

}

