using System;
using UnityEngine;

public class Bar : MonoBehaviour {
	
	public static readonly string BAR_PART_MIDDLE = "Middle"; 
	public static readonly string BAR_PART_END = "End";
	public static readonly string BAR_PART_PROGRESS_BEGIN = "ProgressBegin"; 
	public static readonly string BAR_PART_PROGRESS = "Progress"; 
	public static readonly string BAR_PART_PROGRESS_END = "ProgressEnd";

	protected float percentage = 0;
	

	private bool isVisible_ = false;
	public bool isVisible {
		get {
			return isVisible_;
		}
		set {
			isVisible_ = value;
			updateViewsVisibility();
		} 
	}

	protected virtual void Start() {
		updateViewsVisibility();
	}
		
	public float getPercentage() {
		return percentage;
	}

	public void setValues(int value, int maxValue, bool mustRevealBar) {
	
		setPercentage(value / (float)maxValue, mustRevealBar);
	}

	public virtual void setPercentage(float percentage, bool mustRevealBar) {

		if(percentage < 0) {
			this.percentage = 0;
		} else if(percentage > 1) {
			this.percentage = 1;
		} else {
			this.percentage = percentage;
		}

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

