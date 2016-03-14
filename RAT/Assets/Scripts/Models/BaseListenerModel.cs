using System;
using System.Collections.Generic;
using Node;
using UnityEngine;

public abstract class BaseListenerModel {

	public static List<Listener> getListeners(BasePositionableElement nodeElement) {

		List<Listener> res = new List<Listener>();

		int nbListeners = nodeElement.getListenersCount();
		for(int i = 0 ; i < nbListeners ; i++) {

			NodeListener nodeListener = nodeElement.getListener(i);

			Listener listener = new Listener(nodeListener.nodeIn.value, nodeListener.nodeOut.value);
			res.Add(listener);
		}

		return res;
	}

	private List<Listener> listeners;

	public BaseListenerModel(List<Listener> listeners) : base() {

		if(listeners == null) {
			this.listeners = new List<Listener>();
		} else {
			this.listeners = new List<Listener>(listeners);
		}

	}

	public void trigger(string inputCallName) {

		if(string.IsNullOrEmpty(inputCallName)) {
			return;
		}

		if(listeners == null) {
			return;
		}

		MonoBehaviour mapListener = GameHelper.Instance.getCurrentMapListenerBehaviour();
		if(mapListener == null) {
			// no listener class has been created for this map
			Debug.LogWarning("No listener class has been created for this map, in : " + inputCallName);
			return;
		}


		List<string> coroutinesToStart = new List<string>();

		foreach(Listener listener in listeners) {

			//find out call with in call
			if(!inputCallName.Equals(listener.inputCallName)) {
				//not the good one
				continue;
			}

			string outputCallName = listener.outputCallName;

			if(string.IsNullOrEmpty(outputCallName)) {
				continue;
			}

			coroutinesToStart.Add(outputCallName);
		}

		//start all coroutines in once
		foreach(string coroutineName in coroutinesToStart) {
			mapListener.StartCoroutine(coroutineName);
		}

	}
}

