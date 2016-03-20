using System;
using System.Collections;
using UnityEngine;

public class HUDBar : Bar {

	private static readonly float UI_MULTIPLIER = 0.1f;
	private static readonly float MAX_PERCENTAGE_AVERAGE = 0.6f;//the screen percentage when the max is reached

	private float percentageMax = 1;//the percentage of max (if the player can only reach 2000hp max with the max level and if he has 200hp max now, the current value is 0.1)

	protected override void Start() {

		isVisible = true;

	}

	protected virtual void FixedUpdate() {

		int pixelSize = GameHelper.Instance.getMainCameraResizer().pixelSize;

		if(pixelSize <= 0) {
			return;
		}

		float barWidth = (int)((Screen.width / (float)pixelSize) * percentageMax * MAX_PERCENTAGE_AVERAGE) * UI_MULTIPLIER;
		float minimumWidth = UI_MULTIPLIER * 2;
		if(barWidth < minimumWidth) {
			barWidth = minimumWidth;
		}

		
		//change size of bar parts
		RectTransform middleRectTransform = GetComponent<RectTransform>();
		Vector2 middleSizeDelta = middleRectTransform.sizeDelta;
		middleSizeDelta.x = barWidth;
		middleRectTransform.sizeDelta = middleSizeDelta;

		RectTransform progressRectTransform = transform.Find(BAR_PART_PROGRESS).gameObject.GetComponent<RectTransform>();
		Vector2 progressSizeDelta = progressRectTransform.sizeDelta;
		progressSizeDelta.x = Mathf.FloorToInt(barWidth * percentage / UI_MULTIPLIER) * UI_MULTIPLIER;
		progressRectTransform.sizeDelta = progressSizeDelta;


		updateViewsVisibility();

	}


	public void setBarSize(float screenPercentage) {

		if(screenPercentage <= 0) {
			percentageMax = 0.01f;
		} else {
			percentageMax = screenPercentage;
		}

	}

	public void setValues(int value, int maxValue) {

		base.setValues(value, maxValue, true);
	}
}

