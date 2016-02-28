using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementLoot : BaseIdentifiableNodeElement {
		
		public NodeLabel nodeItem { get ; private set; }
		public NodeInt nodeNbGrouped { get ; private set; }

		public NodeElementLoot (XmlNode node) : base(node) {
			
			nodeItem = parseChild("item", typeof(NodeLabel), true) as NodeLabel;
			nodeNbGrouped = parseChild("nbGrouped", typeof(NodeInt)) as NodeInt;
			
			if(nodeNbGrouped == null) {
				nodeNbGrouped = new NodeInt(1);
			} else if(nodeNbGrouped.value < 1) {
				throw new System.InvalidOperationException("Nb grouped must be 1 or more : " + nodeNbGrouped.value);
			}

		}
		
		public override void freeXmlObjects() {

			nodeItem.freeXmlObjects();
			if(nodeNbGrouped != null) {
				nodeNbGrouped.freeXmlObjects();
			}

			base.freeXmlObjects();
		}

	}
}

