using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractMenuType {
		
	private List<AbstractSubMenuType> subMenuTypes = new List<AbstractSubMenuType>();

	public AbstractMenuType(AbstractSubMenuType[] subMenuTypes) {
		
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
	}

}

