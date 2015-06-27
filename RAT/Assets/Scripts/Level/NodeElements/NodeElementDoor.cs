using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementDoor : BaseNodeElement {
		
		public NodeString nodeId { get ; private set; }
		public NodeOrientation nodeOrientation { get ; private set; }
		public NodeInt nodeSpacing { get ; private set; }
		public NodeDirection nodeUnlockSide { get ; private set; }
		public NodeDoorStatus nodeDoorStatus { get ; private set; }
		public NodeLabel nodeRequire { get ; private set; }

		public NodeElementDoor() : base() {
		}

		public NodeElementDoor (XmlNode node) : base(node) {
			
			nodeId = parseChild("id", typeof(NodeString)) as NodeString;
			nodeOrientation = parseChild("orientation", typeof(NodeOrientation), true) as NodeOrientation;
			nodeSpacing = parseChild("spacing", typeof(NodeInt), true) as NodeInt;
			nodeUnlockSide = parseChild("unlockSide", typeof(NodeDirection)) as NodeDirection;
			nodeDoorStatus = parseChild("status", typeof(NodeDoorStatus), true) as NodeDoorStatus;
			nodeRequire = parseChild("require", typeof(NodeLabel)) as NodeLabel;

			if(nodeSpacing.value < 1) {
				throw new System.InvalidOperationException("Spacing must be 1 or more : " + nodeSpacing.value);
			}

		}
		
		public override void freeXmlObjects() {

			if(nodeId != null) {
				nodeId.freeXmlObjects();
			}
			nodeOrientation.freeXmlObjects();
			nodeSpacing.freeXmlObjects();
			if(nodeUnlockSide != null) {
				nodeUnlockSide.freeXmlObjects();
			}
			nodeDoorStatus.freeXmlObjects();
			if(nodeRequire != null) {
				nodeRequire.freeXmlObjects();
			}
			
			base.freeXmlObjects();
		}

	}
}

