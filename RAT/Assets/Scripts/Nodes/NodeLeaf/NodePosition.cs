using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodePosition : BaseNode {

		public int x { get; private set; }
		public int y { get; private set; }

		public NodePosition() {
		}

		public NodePosition(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();

			if(nodeList.Count > 2) {
				Debug.LogWarning("Nb elements for " + getText() + " > 2 : " + nodeList.Count);
			}

			XmlNode nodeX = nodeList[0];
			string strX = getText(nodeX);
			if(!String.IsNullOrEmpty(strX)) {
				x = int.Parse(strX);
			}

			if(nodeList.Count <= 1) {
				return;
			}

			XmlNode nodeY = nodeList[1];
			string strY = getText(nodeY);
			if(!String.IsNullOrEmpty(strY)) {
				y = int.Parse(strY);
			}

		}


	}

}

