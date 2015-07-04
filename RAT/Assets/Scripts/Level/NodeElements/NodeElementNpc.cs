using System;
using System.Xml;
using UnityEngine;

namespace Level {
	
	public class NodeElementNpc : BaseNodeElement {
				
		public NodeElementNpc() : base() {
		}
		
		public NodeElementNpc(XmlNode node) : base(node) {

		}
		
		public override void freeXmlObjects() {

			base.freeXmlObjects();
		}
	}
}
