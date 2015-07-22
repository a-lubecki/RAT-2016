using System;
using System.Xml;
using UnityEngine;

namespace Level {
	
	public class BaseIdentifiableNodeElement : BaseNodeElement {
		
		public NodeString nodeId { get ; private set; }

		public BaseIdentifiableNodeElement(XmlNode node) : base(node) {
			
			nodeId = parseChild("id", typeof(NodeString)) as NodeString;
			if(nodeId == null) {
				throw new System.InvalidOperationException("Unable to parse node id");
			}
		}
		
		public override void freeXmlObjects() {
			
			if(nodeId != null) {
				nodeId.freeXmlObjects();
			}

			base.freeXmlObjects();
		}
	}
}

