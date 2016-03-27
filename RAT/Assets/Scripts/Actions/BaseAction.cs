using System;
using UnityEngine;

public abstract class BaseAction {

	public readonly IActionnable objectToNotify;
	public readonly string actionLabel;
	public readonly bool hasWarning;
	public readonly bool enabled;

	public BaseAction(IActionnable objectToNotify, string actionLabel) 
		: this(objectToNotify, actionLabel, false, true) {

	}

	public BaseAction(IActionnable objectToNotify, string actionLabel, bool hasWarning, bool enabled) {
		
		if(objectToNotify == null) {
			throw new System.ArgumentException();
		}
		if(string.IsNullOrEmpty(actionLabel)) {
			throw new System.ArgumentException();
		}

		this.objectToNotify = objectToNotify;
		this.actionLabel = actionLabel;
		this.hasWarning = hasWarning;
		this.enabled = enabled;
	}

	public virtual void notifyActionShown() {

		objectToNotify.notifyActionShown(this);
	}

	public virtual void notifyActionHidden() {

		objectToNotify.notifyActionHidden(this);
	}

	public virtual void notifyActionValidated() {

		if(enabled) {
			objectToNotify.notifyActionValidated(this);
		}
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

