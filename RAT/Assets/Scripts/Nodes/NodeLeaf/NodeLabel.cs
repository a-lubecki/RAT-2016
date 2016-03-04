using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeLabel : BaseNode {

		public string value { get; protected set; }

		public NodeLabel(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();

			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getText() + " > 1 : " + nodeList.Count);
			}

			value = getText(nodeList[0]);

			if(string.IsNullOrEmpty(value)) {
				throw new System.InvalidOperationException();
			}
		}
	}
}

