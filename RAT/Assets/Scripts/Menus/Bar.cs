using System;
using UnityEngine;

public class Bar : MonoBehaviour {
	
	private static readonly string OBJECT_NAME_MIDDLE = "Middle"; 
	private static readonly string OBJECT_NAME_PROGRESS = "Progress"; 

	private float percentage = -1;
	
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

		//update :
		float middleWidth = transform.Find(OBJECT_NAME_MIDDLE).localScale.x;
		
		int width = Mathf.FloorToInt(middleWidth * this.percentage);
		
		Transform progress = transform.Find(OBJECT_NAME_PROGRESS);
		Vector2 scale = progress.localScale;
		scale.x = width;
		progress.localScale = scale;

	}


}

