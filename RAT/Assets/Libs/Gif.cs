using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gif : MonoBehaviour {

	private Coroutine coroutineAnimation;

	public float durationSec = 1;
	public bool isLoop;
	
	public Sprite[] sprites;


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
		
		Image image = GetComponent<Image>();
		image.sprite = sprites[0];
	}

	private IEnumerator animate() {
		
		float durationSecTmp = durationSec;
		bool isLoopTmp = isLoop;

		Image image = GetComponent<Image>();

		if(durationSecTmp <= 0) {
			coroutineAnimation = null;
			image.sprite = sprites[0];
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

			image.sprite = sprites[i];

			yield return new WaitForSeconds(secondsToWait);

			if(isLoopTmp && i >= nbImages-1) {
				//reset
				i = -1;
			}
		}

		coroutineAnimation = null;
	
	}

}

