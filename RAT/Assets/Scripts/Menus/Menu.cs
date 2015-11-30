using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private readonly float ANIM_LOOP_COUNT = 10f;

	private Coroutine coroutineOpening;
	private float percentageOpening = 0;

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

	public void open() {
		
		if(percentageOpening >= 1) {
			return;
		}

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
	}

	public void close() {
		
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
