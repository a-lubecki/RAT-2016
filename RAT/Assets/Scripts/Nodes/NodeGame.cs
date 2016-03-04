using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Node {

	public class NodeGame : BaseNode {


		private Dictionary<string, ItemPattern> itemPatternsById = new Dictionary<string, ItemPattern>();
		private Dictionary<ItemType, Dictionary<ItemSubType, List<ItemPattern>>> itemPatternsBySubtypeByType = new Dictionary<ItemType, Dictionary<ItemSubType, List<ItemPattern>>>();


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


					XmlNodeList nodeItemPatternList = getNodeChildren(nodeItemSubType);

					if(nodeItemPatternList.Count <= 0) {
						continue;
					}

					NodeLabel nodeLabelSubType = new NodeLabel(nodeItemSubType);


					for(int iPattern = 0 ; iPattern < nodeItemPatternList.Count ; iPattern++) {

						//WEAPON_KATANA

						XmlNode nodeItemPattern = nodeItemPatternList.Item(iPattern);

						string itemPatternId = getText(nodeItemPattern);
						NodeElementItemPattern nodeElementItemPattern = nodeLabelSubType.parseChild(itemPatternId, typeof(NodeElementItemPattern)) as NodeElementItemPattern;

						addItemPatternNode(nodeElementItemPattern, itemPatternId, itemType, itemSubType);

					}
							
				}

			}


			//free the xml objects from memory
			freeXmlObjects();
		}


		private void addItemPatternNode(NodeElementItemPattern nodeItemPattern, string itemPatternId, ItemType itemType, ItemSubType itemSubType) {

			bool isCastable = (itemType != ItemType.SPECIAL && itemSubType != ItemSubType.WEAPON_CLAWS);//TODO add special heal

			ItemPattern ammoPattern = null;
			NodeLabel nodeAmmoPattern = nodeItemPattern.nodeAmmoPattern;
			if(nodeAmmoPattern != null) {
				ammoPattern = findItemPattern(nodeAmmoPattern.value);
			}

			ItemPattern itemPattern = new ItemPattern(
				itemPatternId,
				itemType, 
				itemSubType, 
				nodeItemPattern.nodeWidth.value,
				nodeItemPattern.nodeHeight.value,
				isCastable,
				ammoPattern,
				nodeItemPattern.nodeMaxGroupable.value
			);

			//add by id
			itemPatternsById.Add(itemPatternId, itemPattern);

			//add by subtype / type :
			Dictionary<ItemSubType, List<ItemPattern>> itemPatternsBySubtype = null;
			if(itemPatternsBySubtypeByType.ContainsKey(itemType)) {
				itemPatternsBySubtype = itemPatternsBySubtypeByType[itemType];
			} else {
				itemPatternsBySubtype = new Dictionary<ItemSubType, List<ItemPattern>>();
				itemPatternsBySubtypeByType.Add(itemType, itemPatternsBySubtype);
			}

			List<ItemPattern> itemPatterns = null;
			if(itemPatternsBySubtype.ContainsKey(itemSubType)) {
				itemPatterns = itemPatternsBySubtype[itemSubType];
			} else {
				itemPatterns = new List<ItemPattern>();
				itemPatternsBySubtype.Add(itemSubType, itemPatterns);
			}

			itemPatterns.Add(itemPattern);
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

