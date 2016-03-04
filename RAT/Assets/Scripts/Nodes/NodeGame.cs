using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Node {

	public class NodeGame : BaseNode {


		private Dictionary<string, ItemPattern> itemPatternsById = new Dictionary<string, ItemPattern>();
		private Dictionary<ItemType, Dictionary<ItemSubType, ItemPattern>> itemPatternsBySubtypeByType = new Dictionary<ItemType, Dictionary<ItemSubType, ItemPattern>>();


		public NodeGame(XmlNode node) : base (node) {


			XmlNodeList nodeItemTypeList = getNodeChildren(node);//root

			for(int iType = 0 ; iType < nodeItemTypeList.Count ; iType++) {

				//WEAPON

				XmlNode nodeItemType = nodeItemTypeList.Item(iType);

				string itemTypeId = getText(nodeItemType);
				ItemType itemType = ItemType.fromString(itemTypeId);

				XmlNodeList nodeItemSubTypeList = getNodeChildren(nodeItemType);

				for(int iSubType = 0 ; iSubType < nodeItemSubTypeList.Count ; iSubType++) {

					//WEAPON_OFFENSIVE

					XmlNode nodeItemSubType = nodeItemSubTypeList.Item(iSubType);

					string itemSubTypeId = getText(nodeItemSubType);
					ItemSubType itemSubType = ItemSubType.fromString(itemType, itemSubTypeId);

					if(getNodeChildren(nodeItemSubType).Count <= 0) {
						//no items under subtype
						continue;
					}

					NodeLabel nodeLabel = new NodeLabel(nodeItemSubType);

					List<BaseNode> nodeItemPatterns = nodeLabel.parseChildren(itemSubTypeId, typeof(NodeElementItemPattern));

					foreach(NodeElementItemPattern nodeItemPattern in nodeItemPatterns) {

						//WEAPON_KATANA

						string itemPatternId = getText(nodeItemPattern);

						addItemPatternNode(nodeItemPattern, itemPatternId, itemType, itemSubType);

					}
							
				}

			}


			//free the xml objects from memory
			freeXmlObjects();
		}


		private void addItemPatternNode(NodeElementItemPattern nodeItemPattern, string itemPatternId, ItemType itemType, ItemSubType itemSubType) {

			bool isCastable = (itemType != ItemType.SPECIAL && itemSubType != ItemSubType.WEAPON_CLAWS);//TODO add others

			ItemPattern ammoPattern = null;
			NodeLabel nodeAmmoType = nodeItemPattern.nodeAmmoType;
			if(nodeAmmoType != null) {
				ammoPattern = findItemPattern(nodeAmmoType.value);
			}

			ItemPattern itemPattern = new ItemPattern(
				itemPatternId, 
				itemPatternId, 
				itemType, 
				itemSubType, 
				nodeItemPattern.nodeWidth.value,
				nodeItemPattern.nodeHeight.value,
				isCastable,
				ammoPattern,
				nodeItemPattern.nodeMaxGroupable.value
			);

			itemPatternsById.Add(itemPatternId, itemPattern);

			Dictionary<ItemSubType, ItemPattern> itemPatternsBySubtype = itemPatternsBySubtypeByType[itemType];
			if(itemPatternsBySubtype == null) {
				itemPatternsBySubtype = new Dictionary<ItemSubType, ItemPattern>();
				itemPatternsBySubtypeByType[itemType] = itemPatternsBySubtype;
			}

			itemPatternsBySubtype.Add(itemSubType, itemPattern);
		}


		public ItemPattern findItemPattern(string id) {

			if(id == null) {
				throw new ArgumentException();
			}

			return itemPatternsById[id];
		}


		public override void freeXmlObjects() {

			base.freeXmlObjects();
		}


	}

}

