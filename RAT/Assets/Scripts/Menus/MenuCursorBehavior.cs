using System;
using UnityEngine;

public class MenuCursorBehavior : MonoBehaviour {


	public bool isVisible { get; private set; }


	protected void Start() {

		hide();
	}

	public void show(GameObject parentGameObject, int width, int height) {

		if(parentGameObject == null) {
			throw new ArgumentException();
		}

		RectTransform parentRectTransform = parentGameObject.GetComponent<RectTransform>();

		float diffX = parentRectTransform.sizeDelta.x / 2f - width * 0.8f;
		if(parentRectTransform.localScale.x < 0) {
			diffX = -diffX;
		}
		Vector2 pos = new Vector2(parentRectTransform.anchoredPosition.x - diffX,
			parentRectTransform.anchoredPosition.y - parentRectTransform.sizeDelta.y / 2f + height * 0.8f);

		show(pos, parentRectTransform, width, height);
	}

	public void show(Vector2 anchoredPosition, RectTransform parentRectTransform, int width, int height) {

		if(parentRectTransform == null) {
			throw new ArgumentException();
		}
		if(width <= 0) {
			throw new ArgumentException();
		}
		if(height <= 0) {
			throw new ArgumentException();
		}

		RectTransform rectTransform = GetComponent<RectTransform>();

		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.anchorMin = parentRectTransform.anchorMin;
		rectTransform.anchorMax = parentRectTransform.anchorMax;
		rectTransform.pivot = parentRectTransform.pivot;
		rectTransform.localScale = new Vector3(
			parentRectTransform.localScale.x < 0 ? -0.8f : 0.8f,
			parentRectTransform.localScale.y < 0 ? -0.8f : 0.8f,
			1);
		rectTransform.sizeDelta = new Vector2(width * 2, height * 2);

		isVisible = true;

		foreach(Transform tr in GetComponentInChildren<Transform>()) {

			tr.gameObject.SetActive(true);
			tr.GetComponent<Gif>().startAnimation();
		}

		//put on top on the UI stack
		rectTransform.SetAsLastSibling();

	}

	public void hide() {

		isVisible = false;

		foreach(Transform tr in GetComponentInChildren<Transform>()) {

			tr.GetComponent<Gif>().stopAnimation();
			tr.gameObject.SetActive(false);
		}

	}


}

