using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Level {

	public class NodeLevel : BaseNode {


		public NodeElementSpawn spawnElement { get; private set; } //can be null

		private List<BaseNode> hubElements;
		private List<BaseNode> linkElements;
		private List<BaseNode> doorElements;


		public NodeLevel(XmlNode node) : base (node) {
			
			spawnElement = parseChild("SPAWN", typeof(NodeElementSpawn)) as NodeElementSpawn;
			
			hubElements = parseChildren("HUB", typeof(NodeElementHub));
			linkElements = parseChildren("LINK", typeof(NodeElementLink));
			doorElements = parseChildren("DOOR", typeof(NodeElementDoor));
			//leverElements = parseChildren("LEVER", typeof(NodeElementLever));
			//buttonElements = parseChildren("BUTTON", typeof(NodeElementButton));
			//lootElements = parseChildren("LOOT", typeof(NodeElementLoot));
			//chestElements = parseChildren("CHEST", typeof(NodeElementChest));
			//npcElements = parseChildren("NPC", typeof(NodeElementNpc));
			
			//free the xml objects from memory
			freeXmlObjects();
			
		}
		
		
		public int getHubCount() {
			return hubElements.Count;
		}
		
		public NodeElementHub getHub(int pos) {
			return hubElements[pos] as NodeElementHub;
		}

		public int getLinkCount() {
			return linkElements.Count;
		}
		
		public NodeElementLink getLink(int pos) {
			return linkElements[pos] as NodeElementLink;
		}
		
		public int getDoorCount() {
			return doorElements.Count;
		}
		
		public NodeElementDoor getDoor(int pos) {
			return doorElements[pos] as NodeElementDoor;
		}

		
		public override void freeXmlObjects() {

			if(spawnElement != null) {
				spawnElement.freeXmlObjects();
			}
			
			foreach(BaseNode node in hubElements) {
				node.freeXmlObjects();
			}
			foreach(BaseNode node in linkElements) {
				node.freeXmlObjects();
			}
			foreach(BaseNode node in doorElements) {
				node.freeXmlObjects();
			}
			
			base.freeXmlObjects();
		}

	}
}

