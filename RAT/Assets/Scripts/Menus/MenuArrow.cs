using System;
using UnityEngine;

public class MenuArrow : ISelectable {

	public static readonly MenuArrow MENU_ARROW_LEFT = new MenuArrow(true);
	public static readonly MenuArrow MENU_ARROW_RIGHT = new MenuArrow(false);

	public bool isLeft { get; private set; }

	public MenuArrow(bool isLeft) {
		this.isLeft = isLeft;
	}

	void ISelectable.onSelect() {

		GameObject arrow; 
		if(isLeft) {
			arrow = GameHelper.Instance.getMenuArrowLeft();
		} else {
			arrow = GameHelper.Instance.getMenuArrowRight();
		}

		GameHelper.Instance.getMenuCursorBehavior().show(arrow, 2, 1);
	}

	void ISelectable.onDeselect() {

		GameHelper.Instance.getMenuCursorBehavior().hide();
	}

	void ISelectable.onSelectionValidated() {

		if(isLeft) {
			GameHelper.Instance.getMenu().selectPreviousSubMenuType();
		} else {
			GameHelper.Instance.getMenu().selectNextSubMenuType();
		}
	}

	void ISelectable.onSelectionCancelled() {
		//do nothing
	}

}

