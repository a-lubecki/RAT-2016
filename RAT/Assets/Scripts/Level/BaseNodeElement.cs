using System;
using System.Xml;
using UnityEngine;

namespace Level {
	
	public class BaseNodeElement : BaseLevelNode {
		
		public NodePosition nodePosition { get ; private set; }
		
		public BaseNodeElement (XmlNode node) : base(node) {

			nodePosition = parseChild("pos", typeof(NodePosition), true) as NodePosition;

		}
	}
}

