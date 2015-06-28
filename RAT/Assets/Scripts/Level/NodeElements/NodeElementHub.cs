using System;
using System.Xml;

namespace Level {
	
	public class NodeElementHub : BaseNodeElement {
		
		public static readonly string LISTENER_CALL_onHubActivated = "onHubActivated";
		public static readonly string LISTENER_CALL_onHubDeactivated = "onHubDeactivated";

		public NodeDirection nodeSpawnDirection { get ; private set; }
		
		public NodeElementHub() : base() {
		}

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

