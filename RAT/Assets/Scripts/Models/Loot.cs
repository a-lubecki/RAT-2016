using System;
using System.Collections.Generic;
using UnityEngine;
using Node;

public class Loot : BaseIdentifiableModel {

	public ItemPattern itemPattern { get; private set; }
	public int nbGrouped { get; private set; }

	public bool isCollected { get; private set; }


	public Loot(NodeElementLoot nodeElementLoot, bool isCollected) 
		: this(nodeElementLoot.nodeId.value, 
			getListeners(nodeElementLoot),
			GameManager.Instance.getNodeGame().findItemPattern(nodeElementLoot.nodeItem.value),
			nodeElementLoot.nodeNbGrouped.value, 
			isCollected) {

	}
		
	public Loot(string id, List<Listener> listeners, ItemPattern itemPattern, int nbGrouped, bool isCollected) : base(id, listeners) {
		
		if(itemPattern == null) {
			throw new ArgumentException("The item pattern was not found for " + id);
		}
		if(nbGrouped <= 0) {
			throw new ArgumentException();
		}

		this.itemPattern = itemPattern;
		this.nbGrouped = nbGrouped;
		this.isCollected = isCollected;
	}


	public void setCollected() {
		isCollected = true;
	}

	public string getLootText() {

		string multiplier = "";
		if(nbGrouped > 1) {
			multiplier = " x" + nbGrouped;
		}
		return itemPattern.getTrName() + multiplier;
	}

	public bool mustReorderBeforePickingUp() {
		return (itemPattern.itemType != ItemType.SPECIAL && 
			!Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT.isNewItemFitting(itemPattern.getFirstGridName(), itemPattern, nbGrouped));
	}

}

