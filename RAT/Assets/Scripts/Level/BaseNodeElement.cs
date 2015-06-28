using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Level {
	
	public class BaseNodeElement : BaseNode {
		
		public NodePosition nodePosition { get ; private set; }
		private List<BaseNode> nodeListeners;

		public BaseNodeElement () : base() {
		}

		public BaseNodeElement (XmlNode node) : base(node) {
			
			nodePosition = parseChild("pos", typeof(NodePosition), true) as NodePosition;
			nodeListeners = parseChildren("listener", typeof(NodeListener));
		}
		
		public int getListenersCount() {
			return nodeListeners.Count;
		}
		
		public NodeListener getListener(int pos) {
			return nodeListeners[pos] as NodeListener;
		}

		public void trigger(string inCallName) {
			
			if(string.IsNullOrEmpty(inCallName)) {
				return;
			}

			if(nodeListeners == null) {
				return;
			}

			MonoBehaviour mapListener = GameHelper.Instance.getCurrentMapListener();
			if(mapListener == null) {
				// no listener class has been created for this map
				Debug.LogWarning("No listener class has been created for this map, in : " + inCallName);
				return;
			}


			List<string> coroutinesToStart = new List<string>();

			foreach(BaseNode node in nodeListeners) {

				NodeListener nodeListener = node as NodeListener;
				
				//find out call with in call
				if(!inCallName.Equals(nodeListener.nodeIn.value)) {
					//not the good one
					continue;
				}
				
				string outCallName = nodeListener.nodeOut.value;
				
				if(string.IsNullOrEmpty(outCallName)) {
					continue;
				}

				coroutinesToStart.Add(outCallName);
			}

			//start all coroutines in once
			foreach(string coroutineName in coroutinesToStart) {
				mapListener.StartCoroutine(coroutineName);
			}

		}
		
		public override void freeXmlObjects() {

			nodePosition.freeXmlObjects();
			
			foreach(BaseNode node in nodeListeners) {
				node.freeXmlObjects();
			}

			base.freeXmlObjects();
		}
	}
}

