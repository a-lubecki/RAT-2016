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
		GameObject actionObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_ACTION);
		GameObject backgroundObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BACKGROUND_MESSAGE_ACTION);
		
		Text textComponent = actionObject.GetComponent<Text>();
		Image imageComponent = backgroundObject.GetComponent<Image>();

		textComponent.enabled = true;
		imageComponent.enabled = true;
		
		textComponent.text = action.actionLabel;

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
		GameObject actionObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_ACTION);
		GameObject backgroundObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BACKGROUND_MESSAGE_ACTION);
		
		Text textComponent = actionObject.GetComponent<Text>();
		Image imageComponent = backgroundObject.GetComponent<Image>();
		
		textComponent.text = "";
		
		textComponent.enabled = false;
		imageComponent.enabled = false;

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

		//retain action because it will be nulled in the hide
		BaseAction retainedAction = action;

		hideAction(retainedAction);

		//notify after hiding because a new action can be shown in this notify, fix actions concurrency
		retainedAction.notifyActionValidated();

		return true;
	}

}

