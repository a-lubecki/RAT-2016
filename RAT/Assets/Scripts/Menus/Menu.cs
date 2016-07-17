﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private readonly float ANIM_LOOP_COUNT_OPEN_CLOSE = 10f;
	private readonly float ANIM_LOOP_COUNT_SWITCH_SUB = 8f;

	private Coroutine coroutineOpening;
	private float percentageOpening = 0;
	
	private AbstractMenuType currentMenuType;

	public MenuSelector menuSelector { get; private set; }

	// Use this for initialization
	void Start () {

		menuSelector = new MenuSelector();

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
		gameObjectTitleLeft.GetComponent<Text>().text = getCurrentSubMenuType().getTrName();//can't be null;
		gameObjectTitleRight.GetComponent<Text>().text = null;

		animateArrows();

		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateOpening());

	}

	private IEnumerator animateOpening() {
		
		PlayerActionsManager.Instance.setEnabled(this, false);

		for(int i = (int)(percentageOpening * ANIM_LOOP_COUNT_OPEN_CLOSE) ; i <= ANIM_LOOP_COUNT_OPEN_CLOSE ; i++) {

			percentageOpening = i / ANIM_LOOP_COUNT_OPEN_CLOSE;

			updateViews();

			yield return new WaitForSeconds(0.01f);
		}

		addSubMenu();

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

		if(menuSelector.isValidated) {
			menuSelector.cancelSelectedItem();
		}

		coroutineOpening = StartCoroutine(animateClosing());
	}
	
	private IEnumerator animateClosing() {

		for(int i = (int)(percentageOpening * ANIM_LOOP_COUNT_OPEN_CLOSE) ; i >= 0 ; i--) {
			
			percentageOpening = i / ANIM_LOOP_COUNT_OPEN_CLOSE;
			
			updateViews();
			
			yield return new WaitForSeconds(0.01f);
		}
		
		removeSubMenu();

		currentMenuType = null;

		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);
		gameObjectTitleLeft.GetComponent<Text>().text = null;
		gameObjectTitleRight.GetComponent<Text>().text = null;
		
		animateArrow(true, false, false);
		animateArrow(false, false, false);

		PlayerActionsManager.Instance.setEnabled(this, true);

		menuSelector.deselectItem();

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

		removeSubMenu();

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

		removeSubMenu();

		menuType.selectNextSubMenuType();

		//animate
		if(coroutineOpening != null) {
			StopCoroutine(coroutineOpening);
		}
		
		coroutineOpening = StartCoroutine(animateNextSubMenuSelection(menuType.getCurrentSubMenuType()));
	}
	
	private IEnumerator animatePreviousSubMenuSelection(AbstractSubMenuType subMenuType) {
		
		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);
		
		if(string.IsNullOrEmpty(gameObjectTitleRight.GetComponent<Text>().text)) {
			gameObjectTitleRight.GetComponent<Text>().text = gameObjectTitleLeft.GetComponent<Text>().text;
		}
		gameObjectTitleLeft.GetComponent<Text>().text = subMenuType.getTrName();
		
		animateArrow(false, false, false);
		animateArrow(true, true, true);
		
		
		float percentageRotation = 0;
		
		for(int i = 0 ; i <= ANIM_LOOP_COUNT_SWITCH_SUB ; i++) {
			
			percentageRotation = i / ANIM_LOOP_COUNT_SWITCH_SUB;
			
			Vector3 scaleLeft = gameObjectTitleLeft.transform.localScale;
			scaleLeft.x = percentageRotation;
			gameObjectTitleLeft.transform.localScale = scaleLeft;
			
			Vector3 scaleRight = gameObjectTitleRight.transform.localScale;
			scaleRight.x = (1 - percentageRotation);
			gameObjectTitleRight.transform.localScale = scaleRight;
			
			yield return new WaitForSeconds(0.01f);
		}
		
		gameObjectTitleRight.GetComponent<Text>().text = null;
		
		animateArrows();
		
		addSubMenu();
		
		coroutineOpening = null;
	}

	private IEnumerator animateNextSubMenuSelection(AbstractSubMenuType subMenuType) {

		GameObject gameObjectTitleLeft = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_LEFT);
		GameObject gameObjectTitleRight = GameObject.Find(Constants.GAME_OBJECT_NAME_SUB_MENU_TITLE_RIGHT);

		if(string.IsNullOrEmpty(gameObjectTitleLeft.GetComponent<Text>().text)) {
			gameObjectTitleLeft.GetComponent<Text>().text = gameObjectTitleRight.GetComponent<Text>().text;
		}
		gameObjectTitleRight.GetComponent<Text>().text = subMenuType.getTrName();
		
		animateArrow(false, true, true);
		animateArrow(true, false, false);


		float percentageRotation = 0;
		
		for(int i = 0 ; i <= ANIM_LOOP_COUNT_SWITCH_SUB ; i++) {
			
			percentageRotation = i / ANIM_LOOP_COUNT_SWITCH_SUB;
			
			Vector3 scaleLeft = gameObjectTitleLeft.transform.localScale;
			scaleLeft.x = (1 - percentageRotation);
			gameObjectTitleLeft.transform.localScale = scaleLeft;
			
			Vector3 scaleRight = gameObjectTitleRight.transform.localScale;
			scaleRight.x = percentageRotation;
			gameObjectTitleRight.transform.localScale = scaleRight;
			
			yield return new WaitForSeconds(0.01f);
		}
		
		gameObjectTitleLeft.GetComponent<Text>().text = null;
		
		animateArrows();

		addSubMenu();

		coroutineOpening = null;
	}

	private void addSubMenu() {
		
		if(currentMenuType == null) {
			return;
		}

		AbstractSubMenuType subMenuType = currentMenuType.getCurrentSubMenuType();

		GameObject subMenuKeeper = GameHelper.Instance.getSubMenuKeeperGameObject();

		Transform subMenuTransform = subMenuKeeper.transform.Find(subMenuType.getGameObjectName());
		if(subMenuTransform == null) {
			return;
		}
		
		subMenuTransform.gameObject.SetActive(true);

		subMenuTransform.SetParent(transform);

		//adjust
		RectTransform rectTransform = subMenuTransform.GetComponent<RectTransform>();

		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(1, 1);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		
		rectTransform.offsetMin = new Vector2(0, 0);
		rectTransform.offsetMax = new Vector2(0, -3);
		rectTransform.rotation = Quaternion.identity;
		rectTransform.localScale = new Vector3(1, 1, 1);


		subMenuType.updateViews(subMenuTransform.gameObject);
		subMenuType.selectFirstItem(true);

	}
	
	private void removeSubMenu() {
		
		if(currentMenuType == null) {
			return;
		}

		GameObject subMenuGameObject = currentMenuType.getCurrentSubMenuType().getSubMenuGameObject(this);
		if(subMenuGameObject == null) {
			return;
		}

		menuSelector.deselectItem();

		GameObject subMenuKeeper = GameHelper.Instance.getSubMenuKeeperGameObject();
		
		subMenuGameObject.SetActive(false);

		subMenuGameObject.transform.SetParent(subMenuKeeper.transform);

	}

	private void animateArrows() {
		
		animateArrow(true, true, false);
		animateArrow(false, true, false);
	}

	private void animateArrow(bool isLeft, bool isAnimating, bool isFast) {


		GameObject arrow;
		if(isLeft) {
			arrow = GameHelper.Instance.getMenuArrowLeft();
		} else {
			arrow = GameHelper.Instance.getMenuArrowRight();
		}

		Gif gif = arrow.GetComponent<Gif>();

		if(isAnimating) {

			if(isFast) {
				gif.durationSec = 0.1f;
			} else {
				gif.durationSec = 0.6f;
			}

			gif.startAnimation();

		} else {

			gif.stopAnimation();
		}

	}


	public void validate() {

		if(menuSelector.isValidated) {
			if(ItemInGridActionsManager.Instance.isShowingActions()) {
				ItemInGridActionsManager.Instance.executeSelectedAction();
			}
			return;
		}

		bool consumed = currentMenuType.getCurrentSubMenuType().validate();

		if(!consumed) {
			menuSelector.validateSelectedItem();
		}

	}
	
	public void cancel() {

		if(menuSelector.isValidated) {

			menuSelector.cancelSelectedItem();

		} else {

			bool consumed = currentMenuType.getCurrentSubMenuType().cancel();

			if(!consumed) {
				closeAny();
			}
		}

	}

	public void navigateUp() {
		
		if(menuSelector.isValidated) {
			if(ItemInGridActionsManager.Instance.isShowingActions()) {
				ItemInGridActionsManager.Instance.selectPreviousAction();
			}
			return;
		}

		bool consumed = currentMenuType.getCurrentSubMenuType().navigateUp();

		if(!consumed) {
			
			ISelectable selected = menuSelector.selectedItem;
			if(selected != null && !(selected is MenuArrow)) {

				if(selected is ItemInGrid) {

					bool isLeft = GameHelper.Instance.getMenu().getCurrentSubMenuType().isLeftGrid((selected as ItemInGrid).getGridName());

					menuSelector.selectItem(this, isLeft ? MenuArrow.MENU_ARROW_LEFT : MenuArrow.MENU_ARROW_RIGHT);
				}
			}
		}
		
	}
	
	public void navigateDown() {
		
		if(menuSelector.isValidated) {
			if(ItemInGridActionsManager.Instance.isShowingActions()) {
				ItemInGridActionsManager.Instance.selectNextAction();
			}
			return;
		}

		ISelectable selected = menuSelector.selectedItem;
		if(selected != null) {
			
			if(selected is MenuArrow) {
				currentMenuType.getCurrentSubMenuType().selectFirstItem((selected as MenuArrow).isLeft);
			} else {
				currentMenuType.getCurrentSubMenuType().navigateDown();
			}

		} else {

			currentMenuType.getCurrentSubMenuType().selectFirstItem(true);
		}

	}
	
	public void navigateRight() {

		if(menuSelector.isValidated) {
			return;
		}

		bool consumed = currentMenuType.getCurrentSubMenuType().navigateRight();

		if(!consumed) {
			menuSelector.selectItem(this, MenuArrow.MENU_ARROW_RIGHT);
		}

	}
	
	public void navigateLeft() {

		if(menuSelector.isValidated) {
			return;
		}

		bool consumed = currentMenuType.getCurrentSubMenuType().navigateLeft();

		if(!consumed) {
			menuSelector.selectItem(this, MenuArrow.MENU_ARROW_LEFT);
		}

	}

}
