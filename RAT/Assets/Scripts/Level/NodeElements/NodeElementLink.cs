using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeElementLink : BaseNodeElement {
		
		public LevelNodeString nodeNextMap { get ; private set; }
		public NodePosition nodeNextPosition { get ; private set; }
		public NodeDirection nodeNextDirection { get ; private set; }

		public NodeElementLink (XmlNode node) : base(node) {
			
			nodeNextMap = parseChild("nextMap", typeof(LevelNodeString)) as LevelNodeString;
			nodeNextPosition = parseChild("nextPos", typeof(NodePosition)) as NodePosition;
			nodeNextDirection = parseChild("nextDirection", typeof(NodeDirection)) as NodeDirection;

		}
	}
}

