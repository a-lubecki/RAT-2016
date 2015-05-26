using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class LevelNodeInt : LevelNode {

		public int value { get; private set; }

		public LevelNodeInt(XmlNode node) : base(node) {
			
			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				throw new System.InvalidOperationException();
			}			
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

