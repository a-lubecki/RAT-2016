using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeDoorStatus : BaseNode {

		public enum DoorStatus {
			CLOSED,
			OPENED
		}

		public DoorStatus value { get; private set; }
		
		public NodeDoorStatus() {
			value = DoorStatus.CLOSED;
		}

		public NodeDoorStatus(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
		
			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getText() + " > 1 : " + nodeList.Count);
			}

			string nodeValue = getText(nodeList[0]);
			
			if(string.IsNullOrEmpty(nodeValue)) {
				throw new System.InvalidOperationException();
			}

			value = (DoorStatus)Enum.Parse(typeof(DoorStatus), nodeValue);
		}

	}
}

