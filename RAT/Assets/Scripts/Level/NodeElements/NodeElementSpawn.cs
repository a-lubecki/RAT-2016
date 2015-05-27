using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementSpawn : BaseNodeElement {

		public NodeDirection nodeDirection { get ; private set; }

		public NodeElementSpawn (XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 1) {
				throw new System.InvalidOperationException();
			}
			if(nodeList.Count > 2) {
				Debug.LogWarning("Nb elements for " + getText() + " > 2 : " + nodeList.Count);
			}

			foreach(XmlNode n in nodeList) {
				
				string label = getText(n);
				
				if("direction".Equals(label)) {
					nodeDirection = new NodeDirection(n);
					break;
				}
			}
			
			if(nodeDirection == null) {
				nodeDirection = new NodeDirection();
			}
		}
	}
}

