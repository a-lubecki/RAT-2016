using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Node {

	public class NodeGame : BaseNode {


		private Dictionary<string, ItemPattern> itemPatternsById = new Dictionary<string, ItemPattern>();
		private Dictionary<ItemType, Dictionary<ItemSubType, List<ItemPattern>>> itemPatternsBySubtypeByType = new Dictionary<ItemType, Dictionary<ItemSubType, List<ItemPattern>>>();


		public NodeGame(XmlNode node) : base (node) {


			List<BaseNode> nodeItemTypeList = parseAllChildren(typeof(BaseNode));

			foreach(BaseNode nodeItemType in nodeItemTypeList) {

				ItemType itemType = ItemType.fromString(getText(nodeItemType));

				List<BaseNode> nodeItemSubTypeList = nodeItemType.parseAllChildren(typeof(BaseNode));

				foreach(BaseNode nodeItemSubType in nodeItemSubTypeList) {
					
					ItemSubType itemSubType = ItemSubType.fromString(itemType, getText(nodeItemSubType));

					List<BaseNode> nodeItemPatternList = nodeItemSubType.parseAllChildren(typeof(NodeElementItemPattern));

					foreach(BaseNode baseNode in nodeItemPatternList) {

						//WEAPON_KATANA
						NodeElementItemPattern nodeElementItemPattern = (NodeElementItemPattern) baseNode;
						addItemPatternNode(nodeElementItemPattern, getText(nodeElementItemPattern), itemType, itemSubType);
					}

				}

			}


			//free the xml objects from memory
			freeXmlObjects();
		}


		private void addItemPatternNode(NodeElementItemPattern nodeItemPattern, string itemPatternId, ItemType itemType, ItemSubType itemSubType) {

			bool isCastable = (itemType != ItemType.SPECIAL && !Constants.ITEM_ID_CLAWS.Equals(itemPatternId) && !Constants.ITEM_ID_REGENERATION.Equals(itemPatternId));

			string imageKey = null;
			NodeString nodeImage = nodeItemPattern.nodeImage;
			if(nodeImage != null) {
				imageKey = nodeImage.value;
			} else {
				imageKey = itemPatternId;
			}

			ItemPattern ammoPattern = null;
			NodeLabel nodeAmmoPattern = nodeItemPattern.nodeAmmoPattern;
			if(nodeAmmoPattern != null) {
				ammoPattern = findItemPattern(nodeAmmoPattern.value);
			}

			ItemPattern itemPattern = new ItemPattern(
				itemPatternId,
				imageKey,
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

			if(!itemPatternsById.ContainsKey(id)) {
				return null;
			}

			return itemPatternsById[id];
		}


		public override void freeXmlObjects() {

			base.freeXmlObjects();
		}


	}

}

