using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementLink : BaseNodeElement {
		
		public NodeString nodeNextMap { get ; private set; }
		public NodePosition nodeNextPosition { get ; private set; }
		public NodeDirection nodeNextDirection { get ; private set; }

		public NodeElementLink(XmlNode node) : base(node) {
			
			nodeNextMap = parseChild("nextMap", typeof(NodeString)) as NodeString;
			nodeNextPosition = parseChild("nextPos", typeof(NodePosition), true) as NodePosition;
			nodeNextDirection = parseChild("nextDirection", typeof(NodeDirection), true) as NodeDirection;

		}
				
		public override void freeXmlObjects() {
			
			if(nodeNextMap != null) {
				nodeNextMap.freeXmlObjects();
			}
			
			nodeNextPosition.freeXmlObjects();
			nodeNextDirection.freeXmlObjects();

			base.freeXmlObjects();
		}
	}
}

