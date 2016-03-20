using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeElementSpawn : BasePositionableElement {

		public NodeCharacterDirection nodeDirection { get ; private set; }
		
		public NodeElementSpawn() : base() {
		}

		public NodeElementSpawn(XmlNode node) : base(node) {

			nodeDirection = parseChild("direction", typeof(NodeCharacterDirection), true) as NodeCharacterDirection;
			 
		}
		
		public override void freeXmlObjects() {
						
			nodeDirection.freeXmlObjects();

			base.freeXmlObjects();
		}
	}
}

