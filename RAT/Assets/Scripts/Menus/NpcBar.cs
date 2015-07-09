using System;
using System.Collections;
using UnityEngine;

public class NpcBar : Bar {

	private Coroutine coroutineRevealBar;

	void Start() {
	
		setVisible(false);
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
	
	private void setVisible(bool visible) {
		
		for(int i = 0 ; i < transform.childCount ; i++) {
			transform.GetChild(i).gameObject.SetActive(visible);
		}
	}

}

