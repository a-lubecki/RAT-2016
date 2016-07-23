using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity {
	

	private readonly BehaviorKeeper<BaseEntityBehavior> behaviorKeeper = new BehaviorKeeper<BaseEntityBehavior>();


	public void addBehavior(BaseEntityBehavior behavior) {

		if(behaviorKeeper.add(behavior)) {
			behavior.onBehaviorAttached();
			behavior.onEntityChanged();
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

	public T findBehavior<T>() where T : BaseEntityBehavior { 

		List<BaseEntityBehavior> behaviors = getBehaviors(); 

		T selecteBehavior = null; 
		foreach (BaseEntityBehavior behavior in behaviors) { 

			if (behavior is T) { 
				selecteBehavior = behavior as T; 
				break; 
			} 
		} 

		return selecteBehavior; 
	} 


	protected void updateBehaviors() {

		foreach(BaseEntityBehavior behavior in getBehaviors()) {
			behavior.onEntityChanged();
		}
	}

	public List<GameObject> getGameObjects() {

		List<BaseEntityBehavior> behaviors = getBehaviors();
		List<GameObject> res = new List<GameObject>(behaviors.Count);

		foreach(BaseEntityBehavior behavior in behaviors) {
			res.Add(behavior.gameObject);
		}

		return res;
	}

	public GameObject findGameObject() {

		List<GameObject> gameObjects = getGameObjects();

		if (gameObjects.Count <= 0) {
			return null;
		}

		return gameObjects[0];
	}

	public GameObject findGameObject<T>() where T : BaseEntityBehavior {

		List<GameObject> playerGameObjects = getGameObjects();

		GameObject playerGameObject = null;
		foreach (GameObject gameObject in playerGameObjects) {

			if (gameObject.GetComponent<T>() != null) {
				playerGameObject = gameObject;
				break;
			}
		}

		return playerGameObject;
	}

}

