using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Node {
	
	public class BasePositionableElement : BaseNode {
		
		public NodePosition nodePosition { get ; private set; }
		private List<BaseNode> nodeListeners;

		public BasePositionableElement () : base() {
		}

		public BasePositionableElement (XmlNode node) : base(node) {
			
			nodePosition = parseChild("pos", typeof(NodePosition), true) as NodePosition;
			nodeListeners = parseChildren("listener", typeof(NodeListener));
		}
		
		public int getListenersCount() {
			return nodeListeners.Count;
		}

		public NodeListener getListener(int pos) {
			return nodeListeners[pos] as NodeListener;
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

