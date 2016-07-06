using System;
using UnityEngine;

public abstract class BaseEntityBehavior : MonoBehaviour {

	protected BaseEntity entity { get; private set; }

	private bool wasActiveAndEnabled = false;


	public virtual void init(BaseEntity entity) {

		if(entity == null) {
			throw new ArgumentException();
		}

		this.entity = entity;

		if(isActiveAndEnabled) {
			entity.addBehavior(this);
		}
	}

	protected virtual void FixedUpdate() {

		if(isActiveAndEnabled) {

			if(!wasActiveAndEnabled) {

				//trigger listener
				if(entity != null) {
					entity.addBehavior(this);
				}

				wasActiveAndEnabled = true;
			}

		} else {

			if(wasActiveAndEnabled) {

				//trigger listener
				if(entity != null) {
					entity.removeBehavior(this);
				}

				wasActiveAndEnabled = false;
			}

		}


	}


	public virtual void onBehaviorAttached() {
		//override if necessary
	}

	public virtual void onBehaviorDetached() {
		//override if necessary
	}

}

