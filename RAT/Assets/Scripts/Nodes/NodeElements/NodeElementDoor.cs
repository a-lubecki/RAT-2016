using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeElementDoor : BaseIdentifiableNodeElement {

		public NodeOrientation nodeOrientation { get ; private set; }
		public NodeInt nodeSpacing { get ; private set; }
		public NodeDoorStatus nodeDoorStatus { get ; private set; }
		public NodeDirection nodeUnlockSide { get ; private set; }
		public NodeLabel nodeRequireItem { get ; private set; }

		public NodeElementDoor (XmlNode node) : base(node) {

			nodeOrientation = parseChild("orientation", typeof(NodeOrientation), true) as NodeOrientation;
			nodeSpacing = parseChild("spacing", typeof(NodeInt), true) as NodeInt;
			nodeDoorStatus = parseChild("status", typeof(NodeDoorStatus), true) as NodeDoorStatus;
			nodeUnlockSide = parseChild("unlockSide", typeof(NodeDirection)) as NodeDirection;
			nodeRequireItem = parseChild("requireItem", typeof(NodeLabel)) as NodeLabel;

			if(nodeSpacing.value < 1) {
				throw new System.InvalidOperationException("Spacing must be 1 or more : " + nodeSpacing.value);
			}

		}
		
		public override void freeXmlObjects() {

			nodeOrientation.freeXmlObjects();
			nodeSpacing.freeXmlObjects();
			nodeDoorStatus.freeXmlObjects();
			if(nodeUnlockSide != null) {
				nodeUnlockSide.freeXmlObjects();
			}
			if(nodeRequireItem != null) {
				nodeRequireItem.freeXmlObjects();
			}
			
			base.freeXmlObjects();
		}

	}
}

