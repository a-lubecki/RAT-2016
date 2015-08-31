using System;
using System.Collections;
using UnityEngine;

public class NpcBar : Bar {

	private Coroutine coroutineRevealBar;


	void Start() {
	
		setVisible(false);
	}
	
	
	protected virtual void FixedUpdate() {
		
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
	
	public override void setPercentage(float percentage) {

		float lastPercentage = getPercentage();

		if(percentage == lastPercentage) {
			return;
		}

		base.setPercentage(percentage);

		//reveal only if it's not the first assign and if the percentage is not 0 
		if(lastPercentage >= 0) {
			if(coroutineRevealBar != null) {
				StopCoroutine(coroutineRevealBar);
			}
			coroutineRevealBar = StartCoroutine(revealBar());
		}
	}

	private IEnumerator revealBar() {
		
		setVisible(true);

		yield return new WaitForSeconds(1f);

		setVisible(false);

	}


}

