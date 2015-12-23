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

}

