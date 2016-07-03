using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorKeeper<T> where T : MonoBehaviour {

	private List<WeakReference> behaviorRefs = new List<WeakReference>();

	/**
	 * Return the first found, still available
	 */
	public T getBehavior() {

		removeDeadReferences();

		T behavior = null;

		foreach(WeakReference reference in behaviorRefs) {
			if(reference.IsAlive) {
				behavior = reference.Target as T;
			}
		}

		return behavior;
	}

	public bool has(T behavior) {

		if(behavior == null) {
			return false;
		}

		removeDeadReferences();

		foreach(WeakReference reference in behaviorRefs) {
			if(reference.IsAlive && reference.Target == behavior) {
				return true;
			}
		}

		return false;
	}


	public void add(T behavior) {

		if(behavior == null) {
			return;
		}

		remove(behavior);

		//insert at the first index to indicate that it's the new behavior that must be used
		behaviorRefs.Insert(0, new WeakReference(behavior));
	}

	public void remove(T behavior) {
					
		if(behavior == null) {
			return;
		}

		removeDeadReferences();

		WeakReference referenceToRemove = null;

		foreach(WeakReference reference in behaviorRefs) {
			if(reference.IsAlive && reference.Target == behavior) {
				referenceToRemove = reference;
				break;
			}
		}

		if(referenceToRemove != null) {
			behaviorRefs.Remove(referenceToRemove);
		}
	}

	public void clear(T behavior) {

		behaviorRefs.Clear();
	}

	private void removeDeadReferences() {

		List<WeakReference> newBehaviorRefs = new List<WeakReference>(behaviorRefs.Count);

		foreach(WeakReference reference in behaviorRefs) {
			if(reference.IsAlive) {
				newBehaviorRefs.Add(reference);
			}
		}

		behaviorRefs = newBehaviorRefs;
	}

}

