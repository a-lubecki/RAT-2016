using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInGridActionsManager {

	private static ItemInGridActionsManager instance;

	private ItemInGridActionsManager() {}

	public static ItemInGridActionsManager Instance {

		get {
			if (instance == null) {
				instance = new ItemInGridActionsManager();
			}
			return instance;
		}
	}


	private List<BaseAction> actions;
	private int selectedActionPos = 0;

	private List<ButtonActionBehavior> buttons;


	public bool isShowingActions() {
		return (actions != null);
	}


	public void showActions(List<BaseAction> actions) {

		if(actions == null) {
			throw new System.ArgumentException();
		}
	
		if(this.actions != null) {
			//can't show another action, must hide the current before
			return;
		}

		hideActions();

		this.actions = new List<BaseAction>(actions);
		buttons = new List<ButtonActionBehavior>();

		int i = 0;
		int nbActions = actions.Count;

		foreach(BaseAction action in actions) {

			//show
			GameObject buttonGameObject = GameHelper.Instance.newGameObjectFromPrefab(GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_BUTTON_ACTION));

			ButtonActionBehavior button = buttonGameObject.GetComponent<ButtonActionBehavior>();
			buttons.Add(button);

			RectTransform buttonRectTransform = buttonGameObject.GetComponent<RectTransform>();
			buttonRectTransform.SetParent(GameHelper.Instance.getForegroundGlassGameObject().transform, false);
			buttonRectTransform.anchoredPosition = new Vector2(0, - 3 * i + nbActions * 0.5f);

			button.show(action);

			//notify
			action.notifyActionShown();

			i++;
		}

		selectedActionPos = 0;
		updateSelectedAction();

	}

	public void hideActions() {

		if(actions == null) {
			return;
		}

		//retain action before nulling it
		List<BaseAction> retainedActions = actions;

		actions = null;
		selectedActionPos= 0;

		foreach(BaseAction action in retainedActions) {
				
			//notify
			action.notifyActionHidden();
		}

		foreach(ButtonActionBehavior button in buttons) {

			button.hide();

			button.transform.SetParent(null, false);
			GameObject.Destroy(button.gameObject);
		}

		buttons = null;

	}

	public bool executeSelectedAction() {

		if(actions == null) {
			return false;
		}

		BaseAction action = actions[selectedActionPos];

		if(!action.enabled) {
			return false;
		}

		//retain action because it will be nulled in the hide
		BaseAction retainedAction = action;

		hideActions();

		//notify after hiding because a new action can be shown in this notify, fix actions concurrency
		retainedAction.notifyActionValidated();

		return true;
	}


	public void selectPreviousAction() {

		if(actions == null) {
			return;
		}

		int nbActions = actions.Count;

		for(int i = selectedActionPos - 1 ; i >= 0 ; i--) {

			if(actions[i].enabled) {
				selectedActionPos = i;
				break;
			}
		}

		updateSelectedAction();
	}

	public void selectNextAction() {

		if(actions == null) {
			return;
		}

		int nbActions = actions.Count;

		for(int i = selectedActionPos + 1 ; i < nbActions ; i++) {

			if(actions[i].enabled) {
				selectedActionPos = i;
				break;
			}
		}

		updateSelectedAction();
	}

	private void updateSelectedAction() {

		if(buttons == null) {
			return;
		}

		foreach(ButtonActionBehavior button in buttons) {
			button.setSelected(false);
		}

		buttons[selectedActionPos].setSelected(true);
	}

}

