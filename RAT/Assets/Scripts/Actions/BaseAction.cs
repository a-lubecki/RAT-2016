using System;
using UnityEngine;

public abstract class BaseAction {

	private IActionnable objectToNotify;
	public string actionLabel { get; private set; }

	public BaseAction(IActionnable objectToNotify, string actionLabel) {
		
		if(objectToNotify == null) {
			throw new System.ArgumentException();
		}
		if(string.IsNullOrEmpty(actionLabel)) {
			throw new System.ArgumentException();
		}

		this.objectToNotify = objectToNotify;
		this.actionLabel = actionLabel;
	}

	public virtual void notifyAction() {

		objectToNotify.notifyAction(this);
	}


	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if(!(obj is BaseAction))
			return false;
		BaseAction other = (BaseAction)obj;
		return objectToNotify == other.objectToNotify && actionLabel.Equals(other.actionLabel);
	}
	

	public override int GetHashCode ()
	{
		unchecked {
			return (objectToNotify != null ? objectToNotify.GetHashCode () : 0) ^ (actionLabel != null ? actionLabel.GetHashCode () : 0);
		}
	}
	
}

