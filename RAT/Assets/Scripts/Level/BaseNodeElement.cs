using System;
using System.Xml;
using UnityEngine;

namespace Level {
	
	public class BaseNodeElement : BaseNode {
		
		public NodePosition nodePosition { get ; private set; }

		public BaseNodeElement () : base() {
		}

		public BaseNodeElement (XmlNode node) : base(node) {

			nodePosition = parseChild("pos", typeof(NodePosition), true) as NodePosition;
		}
		
		public override void freeXmlObjects() {

			nodePosition.freeXmlObjects();
			
			base.freeXmlObjects();
		}
	}
}

