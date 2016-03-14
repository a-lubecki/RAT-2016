using System;
using System.Xml;

namespace Node {
	
	public class NodeElementHub : BasePositionableElement {
		
		public NodeDirection nodeSpawnDirection { get ; private set; }

		public NodeElementHub(XmlNode node) : base(node) {

			nodeSpawnDirection = parseChild("spawnDirection", typeof(NodeDirection), false) as NodeDirection;
		}		
		
		public override void freeXmlObjects() {
			
			if(nodeSpawnDirection != null) {
				nodeSpawnDirection.freeXmlObjects();
			}

			base.freeXmlObjects();
		}

	}
}

