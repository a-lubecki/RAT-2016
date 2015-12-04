using UnityEngine;
using System;
using System.Collections;

public class Menu : MonoBehaviour {

	private readonly float ANIM_LOOP_COUNT = 10f;

	private Coroutine coroutineOpening;
	private float percentageOpening = 0;

	private AbstractMenuType currentMenuType;

	// Use this for initialization
	void Start () {
	
		updateViews();

	}

	
	public bool isOpened() {
		return (percentageOpening > 0);
	}

	public bool isAnimating() {
		return (coroutineOpening != null);
	}

	public void open(AbstractMenuType menuType) {

		if(menuType == null) {
			throw new System.ArgumentException();
		}
		if(currentMenuType != null) {
			//already opened or opening
			return;
		}
		
		currentMenuType = menuType;

		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateOpening());

	}

	private IEnumerator animateOpening() {

		for(int i = (int)(percentageOpening * ANIM_LOOP_COUNT) ; i <= ANIM_LOOP_COUNT ; i++) {

			percentageOpening = i / ANIM_LOOP_COUNT;

			updateViews();

			yield return new WaitForSeconds(0.01f);
		}

		coroutineOpening = null;
		
		PlayerActionsManager.Instance.setEnabled(false);
	}
	
	public void close(Type menuTypeClass) {
		
		if(menuTypeClass == null) {
			throw new System.ArgumentException();
		}
		if(currentMenuType == null) {
			return;
		}
		if(!currentMenuType.GetType().Equals(menuTypeClass)) {
			return;
		}

		closeAny();
	}

	public void closeAny() {
		
		if(percentageOpening <= 0) {
			return;
		}
		
		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateClosing());
	}
	
	private IEnumerator animateClosing() {
		
		for(int i = (int)(percentageOpening * ANIM_LOOP_COUNT) ; i >= 0 ; i--) {
			
			percentageOpening = i / ANIM_LOOP_COUNT;
			
			updateViews();
			
			yield return new WaitForSeconds(0.01f);
		}
		
		coroutineOpening = null;
		currentMenuType = null;

		PlayerActionsManager.Instance.setEnabled(true);
	}


	private void updateViews() {

		float middlePercentage = 0.65f;
		float minHeight = 0.01f;

		float w, h;
		if(percentageOpening <= 0) {
			w = 0;
			h = 0;
		} else if(percentageOpening <= middlePercentage) {
			w = percentageOpening / middlePercentage;
			h = minHeight;
		} else {
			w = 1;
			h = ((percentageOpening - middlePercentage) / (1 - middlePercentage));
		}

		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector3(w, h, 1);

	}

}
