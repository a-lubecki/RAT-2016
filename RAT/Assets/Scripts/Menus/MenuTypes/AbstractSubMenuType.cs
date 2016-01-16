using System;
using UnityEngine;

public abstract class AbstractSubMenuType : Displayable {

	public AbstractSubMenuType(string trKey) : base("SubMenuType." + trKey) {

	}

	public abstract string getGameObjectName();
	
	public GameObject getSubMenuGameObject(Menu menu) {

		if(menu == null) {
			throw new System.ArgumentException();
		}

		Transform transform = menu.transform.Find(getGameObjectName());
		if(transform == null) {
			return null;
		}

		return transform.gameObject;
	}

	public virtual void updateViews(GameObject gameObjectSubMenu) {
	}


	private int selectionLevel = 0;
	
	public int getSelectionLevel() {
		//it's 0 by default, if an item is selected the level is 1, if a subitem is selected the level is 2, etc
		return selectionLevel;
	}
	
	protected void incrementSelectionLevel() {
		selectionLevel++;
	}
	protected void decrementSelectionLevel() {
		selectionLevel--;
	}

	public virtual void validate() {

		if(selectionLevel == 0) {

			incrementSelectionLevel();

			//TODO select
		
		} else {


		}

	}
	
	public virtual void cancel() {

		if(selectionLevel == 1) {
			
			decrementSelectionLevel();
			
			//TODO deselect
			
		} else {

		}

	}
	
	public virtual void navigateUp() {
	}
	
	public virtual void navigateDown() {
	}
	
	public virtual void navigateRight() {
	}
	
	public virtual void navigateLeft() {
	}

}

