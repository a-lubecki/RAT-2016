using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Level {

	public class NodeLevel : BaseLevelNode {


		public NodeElementSpawn spawnElement { get ; private set; } //can be null
		private List<NodeElementHub> hubElements = new List<NodeElementHub>();
		//private List<NodeElementLink> linkElements = new List<NodeElementLink>();


		public NodeLevel(XmlNode node) : base (node) {

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				Debug.LogWarning("No elements in main node");
			}

			int i = 0;
			foreach(XmlNode n in nodeList) {

				string label = getText(n);

				if(String.IsNullOrEmpty(label)) {
					Debug.LogWarning("No label in main node : " + i);
					continue;
				}

				if("SPAWN".Equals(label)) {

					if(spawnElement != null) {
						throw new System.InvalidOperationException("Spawn elment already set");
					}

					spawnElement = new NodeElementSpawn(n);

				} else if("LINK".Equals(label)) {
					
					//linkElements.Add(new NodeElementLink(n));

				} else if("HUB".Equals(label)) {
					
					hubElements.Add(new NodeElementHub(n));

				} else if("DOOR".Equals(label)) {


				} else if("LEVER".Equals(label)) {


				} else if("BUTTON".Equals(label)) {


				} else if("LOOT".Equals(label)) {


				} else if("CHEST".Equals(label)) {


				} else if("NPC".Equals(label)) {


				} else if("ENEMY".Equals(label)) {


				} else {

					Debug.LogWarning("Label not recognized : " + label);
				}

				i++;
			}

		}
		
		/*
		public int getLinkCount() {
			return linkElements.Count;
		}
		
		public NodeElementLink getLink(int pos) {
			return linkElements[pos];
		}*/
		
		public int getHubCount() {
			return hubElements.Count;
		}
		
		public NodeElementHub getHub(int pos) {
			return hubElements[pos];
		}

	}
}

