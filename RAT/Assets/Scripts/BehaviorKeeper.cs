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
			throw new ArgumentException();
		}

		removeDeadReferences();

		foreach(WeakReference reference in behaviorRefs) {
			if(reference.IsAlive && reference.Target == behavior) {
				return true;
			}
		}

		return false;
	}


	public bool add(T behavior) {

		if(behavior == null) {
			throw new ArgumentException();
		}

		bool hadBehavior = has(behavior);

		remove(behavior);

		//insert at the first index to indicate that it's the new behavior that must be used
		behaviorRefs.Insert(0, new WeakReference(behavior));

		return !hadBehavior;
	}

	public bool remove(T behavior) {
					
		if(behavior == null) {
			throw new ArgumentException();
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
			return true;
		}

		return false;
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

