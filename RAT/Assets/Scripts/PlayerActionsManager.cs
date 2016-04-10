using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionsManager {
	
	private static PlayerActionsManager instance;
	
	private PlayerActionsManager() {}
	
	public static PlayerActionsManager Instance {
		
		get {
			if (instance == null) {
				instance = new PlayerActionsManager();
			}
			return instance;
		}
	}


	private BaseAction action;

	private HashSet<object> enabledOwners = new HashSet<object>();

	public bool isEnabled() {
		//no owner has required disabling
		return (enabledOwners.Count <= 0);
	}

	/**
	 * An owner can disable the actions manager, it must
	 */
	public void setEnabled(object owner, bool enabled) {

		if(enabled) {
			enabledOwners.Remove(owner);
		} else {
			enabledOwners.Add(owner);
		}

		if(!isEnabled()) {
			hideAnyAction();
		}

	}

	public void setEnabledForced(bool enabled) {

		if(enabled) {
			enabledOwners.Clear();
		} else {
			enabledOwners.Add(this);
		}

		if(!isEnabled()) {
			hideAnyAction();
		}

	}

	public bool isShowingAction() {
		return (action != null);
	}

	public bool isShowingAction(object objectToNotify) {
		return (action != null && action.objectToNotify == objectToNotify);
	}


	public void showAction(BaseAction action) {

		if(action == null) {
			throw new System.ArgumentException();
		}

		if(!isEnabled()) {
			return;
		}

		if(this.action != null) {
			//can't show another action, must hide the current before
			return;
		}

		this.action = action;

		//show
		GameObject buttonGameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BUTTON_ACTION_DEFAULT);
		ButtonActionBehavior button = buttonGameObject.GetComponent<ButtonActionBehavior>();

		button.show(action);
		button.setSelected(true);

		//notify
		action.notifyActionShown();
	}

	public void hideAction(BaseAction action) {
		
		if(action == null) {
			return;
		}
		
		//check to avoid concurrency
		if(!action.Equals(this.action)) {
			return;
		}

		hideAnyAction();
	}
	
	public void hideAnyAction() {
		
		if(action == null) {
			return;
		}

		//retain action before nulling it
		BaseAction retainedAction = action;

		action = null;
		
		//hide
		GameObject buttonGameObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BUTTON_ACTION_DEFAULT);
		ButtonActionBehavior button = buttonGameObject.GetComponent<ButtonActionBehavior>();

		button.hide();

		//notify
		retainedAction.notifyActionHidden();
	}

	public bool executeShownAction() {
		
		if(!isEnabled()) {
			return false;
		}

		if(action == null) {
			return false;
		}

		if(!action.enabled) {
			return false;
		}

		//retain action because it will be nulled in the hide
		BaseAction retainedAction = action;

		hideAction(retainedAction);

		//notify after hiding because a new action can be shown in this notify, fix actions concurrency
		retainedAction.notifyActionValidated();

		return true;
	}

}

