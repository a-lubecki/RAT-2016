using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class LevelNodeLabel : BaseLevelNode {

		public string value { get; protected set; }

		public LevelNodeLabel(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				throw new System.InvalidOperationException();
			}			
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

