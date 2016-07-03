using System;
using UnityEngine;

public abstract class BaseEntityBehavior : MonoBehaviour {

	protected BaseEntity entity { get; private set; }

	public virtual void init(BaseEntity entity) {

		if(entity == null) {
			throw new ArgumentException();
		}

		this.entity = entity;

		if(isActiveAndEnabled) {
			entity.addBehavior(this);
		}
	}

	void OnEnable() {

		if(entity == null) {
			return;
		}

		entity.addBehavior(this);
	}


	void OnDisable() {

		if(entity == null) {
			return;
		}

		entity.removeBehavior(this);
	}


	public virtual void onBehaviorAttached() {
		//override if necessary
	}

	public virtual void onBehaviorDetached() {
		//override if necessary
	}

}

