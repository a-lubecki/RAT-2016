using System;
using System.Collections;
using UnityEngine;

public class HUDBar : Bar {

	private static readonly float MAX_PERCENTAGE_AVERAGE = 0.5f;//the screen percentage when the max is reached

	private float percentageMax = 1;//the percentage of max (if the player can only reach 2000hp max with the max level and if he has 200hp max now, the current value is 0.1)

	protected override void FixedUpdate() {

		int pixelSize = GameHelper.Instance.getMainCameraResizer().pixelSize;

		if(pixelSize <= 0) {
			return;
		}

		int barWidth = (int)((Screen.width / (float)pixelSize) * percentageMax * MAX_PERCENTAGE_AVERAGE);
		if(barWidth < 5) {
			barWidth = 5;
		}

		//change size of bar parts
		Transform transformMiddle = transform.Find(BAR_PART_MIDDLE);
		Transform transformEnd = transform.Find(BAR_PART_END);
		Transform transformProgressEnd = transform.Find(BAR_PART_PROGRESS_END);
		
		Vector2 middleScale = transformMiddle.localScale;
		middleScale.x = barWidth * Constants.TILE_SIZE;
		transformMiddle.localScale = middleScale;
		
		Vector2 endPos = transformEnd.position;
		endPos.x = transformMiddle.position.x + barWidth;
		transformEnd.position = endPos;

		Vector2 progressEndPos = transformProgressEnd.position;
		progressEndPos.x = transformMiddle.position.x + barWidth;
		transformProgressEnd.position = progressEndPos;

		//update progress part
		base.FixedUpdate();

	}


	public void setBarSize(float screenPercentage) {

		if(screenPercentage <= 0) {
			percentageMax = 0.01f;
		} else {
			percentageMax = screenPercentage;
		}

	}

}

