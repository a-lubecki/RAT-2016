using System;
using System.Xml;
using UnityEngine;

namespace Node {
	
	public class NodeElementNpc : BaseIdentifiableNodeElement {

		public NodeCharacterDirection nodeDirection { get ; private set; }
		public NodeInt nodeLevel { get ; private set; }

		public NodeElementNpc(XmlNode node) : base(node) {

			nodeDirection = parseChild("direction", typeof(NodeCharacterDirection), true) as NodeCharacterDirection;
			nodeLevel = parseChild("level", typeof(NodeInt)) as NodeInt;

			if(nodeLevel == null || nodeLevel.value <= 0) {
				throw new InvalidOperationException("The npc max life must be > 0");
			}

		}
		
		public override void freeXmlObjects() {

			nodeDirection.freeXmlObjects();
			nodeLevel.freeXmlObjects();

			base.freeXmlObjects();
		}
	}
}
