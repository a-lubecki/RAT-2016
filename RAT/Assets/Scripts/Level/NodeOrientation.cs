using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeOrientation : LevelNode {

		public enum Orientation {
			VERTICAL,
			HORIZONTAL
		}

		public Orientation value { get; private set; }

		public NodeOrientation(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				throw new System.InvalidOperationException();
			}			
			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getText() + " > 1 : " + nodeList.Count);
			}

			string nodeValue = getText(nodeList[0]);
			
			if(string.IsNullOrEmpty(nodeValue)) {
				throw new System.InvalidOperationException();
			}

			value = (Orientation)Enum.Parse(typeof(Orientation), nodeValue);
		}

	}
}

