using System;
using UnityEngine;

public class Bar : MonoBehaviour {
	
	public static readonly string BAR_PART_MIDDLE = "Middle"; 
	public static readonly string BAR_PART_END = "End";
	public static readonly string BAR_PART_PROGRESS_BEGIN = "ProgressBegin"; 
	public static readonly string BAR_PART_PROGRESS = "Progress"; 
	public static readonly string BAR_PART_PROGRESS_END = "ProgressEnd";

	protected float percentage = 0;
	
	
	private bool isVisible = true;

	
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
	
	public void setVisible(bool isVisible) {
		
		this.isVisible = isVisible;
		
		updateViewsVisibility();
	}

	protected void updateViewsVisibility() {
		
		Transform progressBegin = transform.Find(BAR_PART_PROGRESS_BEGIN);
		Transform progressEnd = transform.Find(BAR_PART_PROGRESS_END);
		
		for(int i = 0 ; i < transform.childCount ; i++) {
			
			Transform childTransform = transform.GetChild(i);
			
			bool childVisible = isVisible;
			if(childTransform == progressBegin) {
				//hide the progress begin part if no more life
				childVisible = (isVisible && percentage > 0);
			} else if(childTransform == progressEnd) {
				//show the progress end part if life is 100%
				childVisible = (isVisible && percentage >= 1);
			}
			
			childTransform.gameObject.SetActive(childVisible);
		}
	}


}

