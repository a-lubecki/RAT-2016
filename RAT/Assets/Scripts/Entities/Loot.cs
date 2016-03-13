using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Node;

public class Loot : MonoBehaviour, IActionnable {
	
	public NodeElementLoot nodeElementLoot { get; private set; }

	public ItemPattern itemPattern { get; private set; }
	public int nbGrouped { get; private set; }

	public bool isCollecting { get; private set; }
	public bool isCollected { get; private set; }

	
	private CircleCollider2D getTriggerCollider() {
		return GetComponent<CircleCollider2D>();
	}

	public void setNodeElementLoot(NodeElementLoot nodeElementLoot) {
		
		if(nodeElementLoot == null) {
			throw new InvalidOperationException();
		}
		
		this.nodeElementLoot = nodeElementLoot;

	}
	
	public void init(ItemPattern itemPattern, int nbGrouped) {

		if(itemPattern == null) {
			throw new ArgumentException();
		}
		if(nbGrouped <= 0) {
			throw new ArgumentException();
		}

		this.itemPattern = itemPattern;
		this.nbGrouped = nbGrouped;

		GetComponent<Gif>().startAnimation();

	}

	public string getLootText() {

		string multiplier = "";
		if(nbGrouped > 1) {
			multiplier = " x" + nbGrouped;
		}
		return itemPattern.getTrName() + multiplier;
	}

	private bool mustReorderBeforePickingUp() {
		return (itemPattern.itemType != ItemType.SPECIAL && 
			!Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT.isNewItemFitting(itemPattern.getFirstGridName(), itemPattern, nbGrouped));
	}

	public void startCollecting() {

		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		isCollecting = true;

		Menu menu = GameHelper.Instance.getMenu();

		if(mustReorderBeforePickingUp()) {
			
			menu.open(Constants.MENU_TYPE_INVENTORY);
			
			//TODO call endCollecting(true/false) when the menu is closed

		} else {

			endCollecting(true);
		}

	}
	
	public void endCollecting(bool isCollected) {
		
		if(this.isCollected) {
			return;
		}
		if(!isCollecting) {
			return;
		}
		
		isCollecting = false;

		if(isCollected) {
			setCollected();
		} else {
			getTriggerCollider().enabled = !isCollected;
		}

	}

	private void setCollected() {
		
		isCollected = true;
		
		getTriggerCollider().enabled = false;
		
		GetComponent<Gif>().stopAnimation();

		//hide the image, can't disable and destroy the object because it won't be saved with the collected items
		GetComponent<SpriteRenderer>().sprite = null;

		//add to inventory
		ItemInGrid item = new ItemInGrid();

		string gridName = itemPattern.getFirstGridName();

		int[] itemCoords = Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT.getNewItemCoords(gridName, itemPattern);
		if(itemCoords == null) {
			throw new NotSupportedException("TODO manage groupable items");
		}
		item.init(itemPattern, gridName, itemCoords[0], itemCoords[1], nbGrouped);

		/*
		//TODO change the x y coord
		//TODO change don't add in GRID_BAG if type is SPECIAL => add in GRID_SPECIAL

		item.init(itemPattern, itemPattern.getFirstGridName(), 0, 0, nbGrouped);
		*/
		GameManager.Instance.getInventory().addItem(item);

	}


	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}
		
		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}
	
		if(getTriggerCollider().IsTouching(collider)) {

			if(mustReorderBeforePickingUp()) {
				PlayerActionsManager.Instance.showAction(new ActionLootCollectThenReorder(this));
			} else {
				PlayerActionsManager.Instance.showAction(new ActionLootCollect(this));
			}
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}
		
		if(!getTriggerCollider().IsTouching(collider)) {

			PlayerActionsManager.Instance.hideAction(new ActionLootCollectThenReorder(this));
			PlayerActionsManager.Instance.hideAction(new ActionLootCollect(this));
		}

	}


	void IActionnable.notifyActionShown(BaseAction action) {

		//show grid

		GameObject gridObject = GameObject.Find(Constants.GAME_OBJECT_NAME_GRID_COLLECTIBLE_ITEM);

		Image gridImage = gridObject.GetComponent<Image>();
		gridImage.enabled = true;

		InventoryGrid grid = gridObject.GetComponent<InventoryGrid>();
		grid.width = itemPattern.widthInBlocks;
		grid.maxWidth = itemPattern.widthInBlocks;
		grid.height = itemPattern.heightInBlocks;
		grid.maxHeight = itemPattern.heightInBlocks;

		grid.deleteGridViews();
		grid.updateGridViews();

		ItemInGrid item = new ItemInGrid();
		item.init(itemPattern, grid.name, 0, 0, nbGrouped);

		grid.removeItems();
		grid.addItem(item);

	}

	void IActionnable.notifyActionHidden(BaseAction action) {

		//hide grid

		GameObject gridObject = GameObject.Find(Constants.GAME_OBJECT_NAME_GRID_COLLECTIBLE_ITEM);

		Image gridImage = gridObject.GetComponent<Image>();
		gridImage.enabled = false;

		InventoryGrid grid = gridObject.GetComponent<InventoryGrid>();
		grid.deleteGridViews();
		grid.removeItems();

	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		StartCoroutine(delayPlayerAfterAction());

	}

	
	private IEnumerator delayPlayerAfterAction() {
		
		Player player = GameHelper.Instance.getPlayer();
		
		player.disableControls();
		getTriggerCollider().enabled = false;

		
		yield return new WaitForSeconds(0.75f);

		startCollecting();
		
		player.enableControls();

	}


}

