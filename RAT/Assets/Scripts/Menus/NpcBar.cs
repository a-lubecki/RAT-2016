using System;
using System.Collections;
using UnityEngine;

public class NpcBar : Bar {

	private Coroutine coroutineRevealBar;

	
	protected virtual void FixedUpdate() {

		if(!isVisible) {
			return;
		}

		float middleWidth = transform.Find(BAR_PART_MIDDLE).localScale.x;
		
		int width = Mathf.FloorToInt(middleWidth * percentage);
		
		//align on the pixel size
		width -= (width % Constants.TILE_SIZE);
		
		Transform progress = transform.Find(BAR_PART_PROGRESS);
		Vector2 scale = progress.localScale;
		scale.x = width;
		progress.localScale = scale;
		
		updateViewsVisibility();
	}

	public override void setPercentage(float percentage, bool mustRevealBar) {

		float lastPercentage = getPercentage();

		if(percentage == lastPercentage) {
			return;
		}

		base.setPercentage(percentage, mustRevealBar);

		if(mustRevealBar) {
			if(coroutineRevealBar != null) {
				StopCoroutine(coroutineRevealBar);
			}
			coroutineRevealBar = StartCoroutine(revealBar());
		}
	}

	private IEnumerator revealBar() {
		
		isVisible = true;

		yield return new WaitForSeconds(1f);

		isVisible = false;

	}


}

