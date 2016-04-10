using System;
using UnityEngine;
using UnityEngine.UI;


public class ButtonActionBehavior : MonoBehaviour {

	private BaseAction action;

	public bool isVisible { get; private set; }
	public bool isSelected { get; private set; }

	void Start() {

		hide();
	}

	public void show(BaseAction action) {

		if(action == null) {
			throw new ArgumentException();
		}

		this.action = action;
		isVisible = true;

		Text textComponent = GetComponentInChildren<Text>();
		textComponent.text = action.actionLabel;

		updateViews();

	}

	public void hide() {

		action = null;
		isVisible = false;

		setSelected(false);

		Text textComponent = GetComponentInChildren<Text>();
		textComponent.text = "";

		updateViews();

	}

	public void setSelected(bool isSelected) {
		
		this.isSelected = isSelected;
	
		updateViews();

	}


	public void updateViews() {

		Text textComponent = GetComponentInChildren<Text>();
		Image backgroundComponent = GetComponent<Image>();

		foreach(MaskableGraphic mGraphic in GetComponentsInChildren<MaskableGraphic>()) {

			bool isSelectionObject = mGraphic.name.Equals(Constants.GAME_OBJECT_NAME_BUTTON_SELECTION_BOTTOM) ||
				mGraphic.name.Equals(Constants.GAME_OBJECT_NAME_BUTTON_SELECTION_TOP) ||
				mGraphic.name.Equals(Constants.GAME_OBJECT_NAME_BUTTON_SELECTION_RIGHT) ||
				mGraphic.name.Equals(Constants.GAME_OBJECT_NAME_BUTTON_SELECTION_LEFT);

			mGraphic.enabled = isVisible && (!isSelectionObject || isSelected);

			if(mGraphic.enabled && mGraphic != backgroundComponent) {

				Color textColor = Color.cyan;
				if(!action.enabled) {
					textColor = Color.gray;
				} else if(action.hasWarning) {
					textColor = Color.yellow;
				}

				mGraphic.color = textColor;
			}

		}

	}

}

