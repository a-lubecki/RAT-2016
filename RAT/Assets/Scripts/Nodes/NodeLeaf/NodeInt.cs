using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeInt : BaseNode {

		public int value { get; private set; }
		
		public NodeInt(int value) {
			this.value = value;
		}

		public NodeInt(XmlNode node) : base(node) {
			
			XmlNodeList nodeList = getNodeChildren();
		
			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getText() + " > 1 : " + nodeList.Count);
			}

			string nodeValue = getText(nodeList[0]);
			
			if(String.IsNullOrEmpty(nodeValue)) {
				throw new System.InvalidOperationException();
			}

			value = int.Parse(nodeValue);
		}
	}
}

