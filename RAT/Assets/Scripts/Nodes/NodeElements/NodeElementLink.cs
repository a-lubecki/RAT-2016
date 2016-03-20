using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeElementLink : BasePositionableElement {
		
		public NodeInt nodeWidth { get ; private set; }
		public NodeInt nodeHeight { get ; private set; }
		public NodeString nodeNextMap { get ; private set; }
		public NodePosition nodeNextPosition { get ; private set; }
		public NodeCharacterDirection nodeNextDirection { get ; private set; }

		public NodeElementLink(XmlNode node) : base(node) {
			
			nodeWidth = parseChild("width", typeof(NodeInt)) as NodeInt;
			if(nodeWidth != null && nodeWidth.value < 1) {
				throw new System.InvalidOperationException("The width is lower than 1 : " + nodeWidth);
			}
			nodeHeight = parseChild("height", typeof(NodeInt)) as NodeInt;
			if(nodeHeight != null && nodeHeight.value < 1) {
				throw new System.InvalidOperationException("The height is lower than 1 : " + nodeWidth);
			}
			nodeNextMap = parseChild("nextMap", typeof(NodeString)) as NodeString;
			nodeNextPosition = parseChild("nextPos", typeof(NodePosition), true) as NodePosition;
			nodeNextDirection = parseChild("nextDirection", typeof(NodeCharacterDirection), true) as NodeCharacterDirection;

		}

		public override void freeXmlObjects() {
			
			if(nodeWidth != null) {
				nodeWidth.freeXmlObjects();
			}
			if(nodeHeight != null) {
				nodeHeight.freeXmlObjects();
			}
			if(nodeNextMap != null) {
				nodeNextMap.freeXmlObjects();
			}
			
			nodeNextPosition.freeXmlObjects();
			nodeNextDirection.freeXmlObjects();

			base.freeXmlObjects();
		}
	}
}

