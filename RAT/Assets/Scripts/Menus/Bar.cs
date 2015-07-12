using System;
using UnityEngine;

public class Bar : MonoBehaviour {
	
	public static readonly string BAR_PART_MIDDLE = "Middle"; 
	public static readonly string BAR_PART_END = "End";
	public static readonly string BAR_PART_PROGRESS = "Progress"; 

	private float percentage = 0;


	protected virtual void FixedUpdate() {

		float middleWidth = transform.Find(BAR_PART_MIDDLE).localScale.x;

		int width = Mathf.FloorToInt(middleWidth * percentage);

		//align on the pixel size
		width -= (width % Constants.TILE_SIZE);

		Transform progress = transform.Find(BAR_PART_PROGRESS);
		Vector2 scale = progress.localScale;
		scale.x = width;
		progress.localScale = scale;
	}

	public float getPercentage() {
		return percentage;
	}

	public void setValues(int value, int maxValue) {
	
		setPercentage(value / (float)maxValue);
	}

	public virtual void setPercentage(float percentage) {

		if(percentage < 0) {
			this.percentage = 0;
		} else if(percentage > 1) {
			this.percentage = 1;
		} else {
			this.percentage = percentage;
		}

	}


}

