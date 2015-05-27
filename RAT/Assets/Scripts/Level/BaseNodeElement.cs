using System;
using System.Xml;
using UnityEngine;

namespace Level {
	
	public class BaseNodeElement : BaseLevelNode {
		
		public NodePosition nodePosition { get ; private set; }
		
		public BaseNodeElement (XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				throw new System.InvalidOperationException();
			}

			foreach(XmlNode n in nodeList) {

				string label = getText(n);

				if("pos".Equals(label)) {
					nodePosition = new NodePosition(n);
					break;
				}
			}

			if(nodePosition == null) {
				nodePosition = new NodePosition();
			}

		}
	}
}

