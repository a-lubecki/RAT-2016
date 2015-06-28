using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeListener : BaseNode {
		
		public NodeString nodeIn { get; private set; }
		public NodeString nodeOut { get; private set; }

		public NodeListener(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
		
			if(nodeList.Count != 2) {
				Debug.LogWarning("Nb elements for " + getText() + " != 2 : " + nodeList.Count);
			}
			
			nodeIn = parseChild("in", typeof(NodeString)) as NodeString;
			nodeOut = parseChild("out", typeof(NodeString)) as NodeString;

		}
		
		public override void freeXmlObjects() {
			
			nodeIn.freeXmlObjects();
			nodeOut.freeXmlObjects();
			
			base.freeXmlObjects();
		}
	}
}

