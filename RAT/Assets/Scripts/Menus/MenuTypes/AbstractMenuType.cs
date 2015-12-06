using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractMenuType {
	
	private List<AbstractSubMenuType> subMenuTypes = new List<AbstractSubMenuType>();
	
	private int currentSubMenuPos = 0;

	private Color color;


	public AbstractMenuType(AbstractSubMenuType[] subMenuTypes, Color color) {
		
		if(subMenuTypes == null) {
			throw new System.ArgumentException();
		}
		if(subMenuTypes.Length <= 0) {
			throw new System.ArgumentException();
		}

		foreach(AbstractSubMenuType subMenuType in subMenuTypes) {

			if(subMenuType == null) {
				throw new System.ArgumentException();
			}

			this.subMenuTypes.Add(subMenuType);
		}

		if(this.subMenuTypes.Count <= 0) {
			throw new System.InvalidOperationException("There must be at least one submenu");
		}

		//defensive copy
		this.color = new Color(color.r, color.g, color.b);
	}


	public AbstractSubMenuType getCurrentSubMenuType() {
		return subMenuTypes[currentSubMenuPos];
	}

	public void selectPreviousSubMenuType() {

		if(currentSubMenuPos > 0) {
			currentSubMenuPos--;
		} else {
			currentSubMenuPos = subMenuTypes.Count - 1;
		}
	}

	public void selectNextSubMenuType() {
		
		if(currentSubMenuPos < subMenuTypes.Count - 1) {
			currentSubMenuPos++;
		} else {
			currentSubMenuPos = 0;
		}
	}

	public Color getColor() {
		//immutable object
		return new Color(color.r, color.g, color.b);
	}

}

