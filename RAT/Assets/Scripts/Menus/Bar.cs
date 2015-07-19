using System;
using UnityEngine;

public class Bar : MonoBehaviour {
	
	public static readonly string BAR_PART_MIDDLE = "Middle"; 
	public static readonly string BAR_PART_END = "End";
	public static readonly string BAR_PART_PROGRESS_BEGIN = "ProgressBegin"; 
	public static readonly string BAR_PART_PROGRESS = "Progress"; 
	public static readonly string BAR_PART_PROGRESS_END = "ProgressEnd";

	private float percentage = 0;

	private bool isVisible = true;

	protected virtual void FixedUpdate() {

		float middleWidth = transform.Find(BAR_PART_MIDDLE).localScale.x;

		int width = Mathf.FloorToInt(middleWidth * percentage);

		//align on the pixel size
		width -= (width % Constants.TILE_SIZE);

		Transform progress = transform.Find(BAR_PART_PROGRESS);
		Vector2 scale = progress.localScale;
		scale.x = width;
		progress.localScale = scale;
		
		//hide the progress begin part if no more life
		Transform progressBegin = transform.Find(BAR_PART_PROGRESS_BEGIN);
		progressBegin.gameObject.SetActive(isVisible && percentage > 0);
		
		//show the progress end part if life is 100%
		Transform progressEnd = transform.Find(BAR_PART_PROGRESS_END);
		progressEnd.gameObject.SetActive(isVisible && percentage >= 1);

	}
	
	public void setVisible(bool visible) {
		
		isVisible = visible;
		
		for(int i = 0 ; i < transform.childCount ; i++) {
			transform.GetChild(i).gameObject.SetActive(visible);
		}
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

