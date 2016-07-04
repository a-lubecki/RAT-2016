﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity {
	

	private readonly BehaviorKeeper<BaseEntityBehavior> behaviorKeeper = new BehaviorKeeper<BaseEntityBehavior>();


	public void addBehavior(BaseEntityBehavior behavior) {

		if(behaviorKeeper.add(behavior)) {
			behavior.onBehaviorAttached();
		}
	}

	public void removeBehavior(BaseEntityBehavior behavior) {

		if(behaviorKeeper.remove(behavior)) {
			behavior.onBehaviorDetached();
		}
	}

	public List<BaseEntityBehavior> getBehaviors() {
		return behaviorKeeper.getBehaviors();
	}


}

