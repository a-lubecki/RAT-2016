using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Level {

	public class NodeLevel : BaseNode {

		
		public NodeElementSpawn spawnElement { get; private set; } //can be null
		public NodeElementHub hubElement { get; private set; } //can be null

		private List<BaseNode> linkElements;
		private List<BaseNode> doorElements;
		//private List<BaseNode> leverElements;
		//private List<BaseNode> buttonElements;
		private List<BaseNode> lootElements;
		//private List<BaseNode> chestElements;
		private List<BaseNode> npcElements;


		public NodeLevel(XmlNode node) : base (node) {
			
			spawnElement = parseChild("SPAWN", typeof(NodeElementSpawn)) as NodeElementSpawn;
			hubElement = parseChild("HUB", typeof(NodeElementHub)) as NodeElementHub;

			linkElements = parseChildren("LINK", typeof(NodeElementLink));
			doorElements = parseChildren("DOOR", typeof(NodeElementDoor));
			//leverElements = parseChildren("LEVER", typeof(NodeElementLever));
			//buttonElements = parseChildren("BUTTON", typeof(NodeElementButton));
			lootElements = parseChildren("LOOT", typeof(NodeElementLoot));
			//chestElements = parseChildren("CHEST", typeof(NodeElementChest));
			npcElements = parseChildren("NPC", typeof(NodeElementNpc));
			
			//free the xml objects from memory
			freeXmlObjects();
			
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
		/*
		public int getLeverCount() {
			return leverElements.Count;
		}
		
		public NodeElementLever getLever(int pos) {
			return leverElements[pos] as NodeElementLever;
		}
		
		public int getButtonCount() {
			return buttonElements.Count;
		}
		
		public NodeElementButton getButton(int pos) {
			return buttonElements[pos] as NodeElementButton;
		}
		*/
		public int getLootCount() {
			return lootElements.Count;
		}
		
		public NodeElementLoot getLoot(int pos) {
			return lootElements[pos] as NodeElementLoot;
		}
		/*
		public int getChestCount() {
			return chestElements.Count;
		}
		
		public NodeElementChest getDoor(int pos) {
			return chestElements[pos] as NodeElementChest;
		}
		*/
		
		public int getNpcCount() {
			return npcElements.Count;
		}
		
		public NodeElementNpc getNpc(int pos) {
			return npcElements[pos] as NodeElementNpc;
		}


		public override void freeXmlObjects() {
			
			if(spawnElement != null) {
				spawnElement.freeXmlObjects();
			}
			if(hubElement != null) {
				hubElement.freeXmlObjects();
			}

			foreach(BaseNode node in linkElements) {
				node.freeXmlObjects();
			}
			foreach(BaseNode node in doorElements) {
				node.freeXmlObjects();
			}
			/*
			foreach(BaseNode node in leverElements) {
				node.freeXmlObjects();
			}
			foreach(BaseNode node in buttonElements) {
				node.freeXmlObjects();
			}*/
			foreach(BaseNode node in lootElements) {
				node.freeXmlObjects();
			}
			/*foreach(BaseNode node in chestElements) {
				node.freeXmlObjects();
			}
			*/
			foreach(BaseNode node in npcElements) {
				node.freeXmlObjects();
			}

			base.freeXmlObjects();
		}

	}
}

