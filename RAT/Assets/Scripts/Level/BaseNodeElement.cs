using System;
using System.Xml;
using UnityEngine;

namespace Level {
	
	public class BaseNodeElement : BaseNode {
		
		public NodePosition nodePosition { get ; private set; }
		public NodeListener nodeListener { get ; private set; }

		public BaseNodeElement () : base() {
		}

		public BaseNodeElement (XmlNode node) : base(node) {
			
			nodePosition = parseChild("pos", typeof(NodePosition), true) as NodePosition;
			nodeListener = parseChild("listener", typeof(NodeListener)) as NodeListener;
		}

		public void trigger(string inCallName) {
			
			if(string.IsNullOrEmpty(inCallName)) {
				return;
			}

			//find out call with in call
			if(!inCallName.Equals(nodeListener.nodeIn.value)) {
				//not the good one
				return;
			}

			string outCallName = nodeListener.nodeOut.value;

			if(string.IsNullOrEmpty(outCallName)) {
				return;
			}

			MonoBehaviour mapListener = GameHelper.Instance.getCurrentMapListener();
			if(mapListener == null) {
				// no listener class has been created for this map
				Debug.LogWarning("No listener class has been created for this map, in : " + inCallName + ", out : " + outCallName);
				return;
			}

			mapListener.StartCoroutine(outCallName);

		}
		
		public override void freeXmlObjects() {

			nodePosition.freeXmlObjects();

			if(nodeListener != null) {
				nodeListener.freeXmlObjects();
			}
			
			base.freeXmlObjects();
		}
	}
}

