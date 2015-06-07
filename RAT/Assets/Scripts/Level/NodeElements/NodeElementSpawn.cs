using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementSpawn : BaseNodeElement {

		public NodeDirection nodeDirection { get ; private set; }
		
		public NodeElementSpawn () : base() {
		}

		public NodeElementSpawn (XmlNode node) : base(node) {

			nodeDirection = parseChild("direction", typeof(NodeDirection), true) as NodeDirection;
			 
		}
		
		public override void freeXmlObjects() {
						
			nodeDirection.freeXmlObjects();

			base.freeXmlObjects();
		}
	}
}

