﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private readonly float ANIM_LOOP_COUNT_OPEN_CLOSE = 10f;
	private readonly float ANIM_LOOP_COUNT_SWITCH_SUB = 5f;

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
		if(isAnimating()) {
			//can't open if animating
			return;
		}

		currentMenuType = menuType;

		Color nextColor = menuType.getColor();
		GetComponent<Image>().color = new Color(nextColor.r, nextColor.g, nextColor.b, 0.8f);

		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);
		gameObjectTitleLeft.GetComponent<Text>().text = getCurrentSubMenuType().getName();//can't be null;
		gameObjectTitleRight.GetComponent<Text>().text = null;

		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateOpening());

	}

	private IEnumerator animateOpening() {
		
		PlayerActionsManager.Instance.setEnabled(false);

		for(int i = (int)(percentageOpening * ANIM_LOOP_COUNT_OPEN_CLOSE) ; i <= ANIM_LOOP_COUNT_OPEN_CLOSE ; i++) {

			percentageOpening = i / ANIM_LOOP_COUNT_OPEN_CLOSE;

			updateViews();

			yield return new WaitForSeconds(0.01f);
		}
		
		PlayerActionsManager.Instance.setEnabled(true);

		coroutineOpening = null;
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
		
		if(isAnimating()) {
			//can't close if animating
			return;
		}
		if(percentageOpening <= 0) {
			return;
		}

		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateClosing());
	}
	
	private IEnumerator animateClosing() {
		
		PlayerActionsManager.Instance.setEnabled(false);

		for(int i = (int)(percentageOpening * ANIM_LOOP_COUNT_OPEN_CLOSE) ; i >= 0 ; i--) {
			
			percentageOpening = i / ANIM_LOOP_COUNT_OPEN_CLOSE;
			
			updateViews();
			
			yield return new WaitForSeconds(0.01f);
		}

		currentMenuType = null;

		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);
		gameObjectTitleLeft.GetComponent<Text>().text = null;
		gameObjectTitleRight.GetComponent<Text>().text = null;

		PlayerActionsManager.Instance.setEnabled(true);

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
			h = (percentageOpening - middlePercentage) / (1 - middlePercentage);
		}

		GetComponent<RectTransform>().localScale = new Vector3(w, h, 1);

		//update title height
		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);
		gameObjectTitleLeft.GetComponent<RectTransform>().localScale = new Vector3(1, percentageOpening, 1);
		gameObjectTitleRight.GetComponent<RectTransform>().localScale = new Vector3(1, percentageOpening, 1);
	}

	
	public AbstractMenuType getCurrentMenuType() {
		return currentMenuType;
	}
	
	public AbstractSubMenuType getCurrentSubMenuType() {
		
		AbstractMenuType menuType = getCurrentMenuType();
		if(menuType == null) {
			return null;
		}
		
		return menuType.getCurrentSubMenuType();
	}
	
	public void selectPreviousSubMenuType() {
		
		AbstractMenuType menuType = getCurrentMenuType();
		if(menuType == null) {
			return;
		}

		menuType.selectPreviousSubMenuType();

		//animate
		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animatePreviousSubMenuSelection(menuType.getCurrentSubMenuType()));
	}

	public void selectNextSubMenuType() {
		
		AbstractMenuType menuType = getCurrentMenuType();
		if(menuType == null) {
			return;
		}
		
		menuType.selectNextSubMenuType();
		
		//animate
		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateNextSubMenuSelection(menuType.getCurrentSubMenuType()));
	}
	
	private IEnumerator animatePreviousSubMenuSelection(AbstractSubMenuType subMenuType) {
		
		PlayerActionsManager.Instance.setEnabled(true);

		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);

		gameObjectTitleLeft.GetComponent<Text>().text = null;
		gameObjectTitleRight.GetComponent<Text>().text = subMenuType.getName();


		float percentageRotation = 0;
		
		for(int i = (int)(percentageRotation * ANIM_LOOP_COUNT_SWITCH_SUB) ; i <= ANIM_LOOP_COUNT_SWITCH_SUB ; i++) {
			
			percentageRotation = i / ANIM_LOOP_COUNT_SWITCH_SUB;
			
			updateTitle(percentageRotation);
			
			yield return new WaitForSeconds(0.01f);
		}
		
		gameObjectTitleLeft.GetComponent<Text>().text = subMenuType.getName();
		gameObjectTitleRight.GetComponent<Text>().text = null;

		PlayerActionsManager.Instance.setEnabled(true);
		
		coroutineOpening = null;
	}
	
	private IEnumerator animateNextSubMenuSelection(AbstractSubMenuType subMenuType) {
		
		PlayerActionsManager.Instance.setEnabled(true);
		
		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);
		
		gameObjectTitleLeft.GetComponent<Text>().text = subMenuType.getName();
		gameObjectTitleRight.GetComponent<Text>().text = null;
		
		
		float percentageRotation = 0;
		
		for(int i = (int)(percentageRotation * ANIM_LOOP_COUNT_SWITCH_SUB) ; i <= ANIM_LOOP_COUNT_SWITCH_SUB ; i++) {
			
			percentageRotation = i / ANIM_LOOP_COUNT_SWITCH_SUB;
			
			updateTitle(percentageRotation);
			
			yield return new WaitForSeconds(0.01f);
		}
		
		gameObjectTitleLeft.GetComponent<Text>().text = null;
		gameObjectTitleRight.GetComponent<Text>().text = subMenuType.getName();
		
		PlayerActionsManager.Instance.setEnabled(true);
		
		coroutineOpening = null;
	}

	private void updateTitle(float percentageRotation) {
		/*
		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);

		if() {

		}*/

	}

}
