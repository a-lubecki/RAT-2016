using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementLoot : BaseIdentifiableNodeElement {

		//TODO node item

		public NodeElementLoot (XmlNode node) : base(node) {

		}
		
		public override void freeXmlObjects() {

			base.freeXmlObjects();
		}

	}
}

