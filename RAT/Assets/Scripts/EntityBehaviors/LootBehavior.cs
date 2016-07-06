using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Node;

public class LootBehavior : BaseEntityBehavior, IActionnable {

	public Loot loot {
		get {
			return (Loot) entity;
		}
	}

	public bool isCollecting { get; private set; }

	private bool isColliding = false;

	public void init(Loot loot) {

		GetComponent<Gif>().startAnimation();

		base.init(loot);

	}

	private CircleCollider2D getTriggerActionInCollider() {
		return GetComponents<CircleCollider2D>()[0];
	}

	private CircleCollider2D getTriggerActionOutCollider() {
		return GetComponents<CircleCollider2D>()[1];
	}

	public void startCollecting() {

		if(loot.isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		isCollecting = true;

		Menu menu = GameHelper.Instance.getMenu();

		if(loot.mustReorderBeforeCollecting()) {
			
			menu.open(Constants.MENU_TYPE_INVENTORY);
			
			//TODO call endCollecting(true/false) when the menu is closed

			isCollecting = false;//TODO TEST
			getTriggerActionInCollider().enabled = true;//TODO TEST

		} else {

			endCollecting(true);
		}

	}
	
	public void endCollecting(bool isCollected) {
		
		if(loot.isCollected) {
			return;
		}
		if(!isCollecting) {
			return;
		}
		
		isCollecting = false;

		if(isCollected) {
			setCollected();
		} else {
			getTriggerActionInCollider().enabled = !isCollected;
		}

	}

	private void setCollected() {
		
		bool hasAddedItemInInventory = false;

		//add to inventory
		AbstractSubMenuType subMenuType = loot.itemPattern.getFirstSubMenuType();
		string gridName = loot.itemPattern.getFirstGridName();

		Inventory inventory = GameManager.Instance.getInventory();
		InventoryGrid inventoryGrid = subMenuType.findInventoryGrid(gridName);

		int remainingNbGrouped = loot.nbGrouped;

		while(remainingNbGrouped > 0) {
			
			ItemInGrid currentGroupableItem = inventoryGrid.getGroupableItem(loot.itemPattern);

			ItemInGrid newItemInGrid;

			if(currentGroupableItem != null) {

				newItemInGrid = currentGroupableItem.newGroupedItem(loot.itemPattern, remainingNbGrouped);

				//replace item
				inventory.removeItem(currentGroupableItem);

			} else {
				
				int[] itemCoords = inventoryGrid.getNewItemCoords(loot.itemPattern);
				if(itemCoords == null) {
					Debug.Log("Can't add new object, lack of blocks");
					break;
				}

				//split nbgrouped vs maxgroupable
				int nb = remainingNbGrouped;
				if(nb > loot.itemPattern.maxGroupable) {
					nb = loot.itemPattern.maxGroupable;
				}

				newItemInGrid = new ItemInGrid(loot.itemPattern, gridName, itemCoords[0], itemCoords[1], (Orientation)itemCoords[2], nb);

			}

			inventory.addItem(newItemInGrid);

			hasAddedItemInInventory = true;

			remainingNbGrouped -= newItemInGrid.getNbGrouped();
		}


		if(hasAddedItemInInventory) {

			loot.setCollected();

			getTriggerActionInCollider().enabled = false;

			GetComponent<Gif>().stopAnimation();

			//hide the image, can't disable and destroy the object because it won't be saved with the collected items
			GetComponent<SpriteRenderer>().sprite = null;
		}

	}


	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(isColliding) {
			return;
		}
		
		if(loot.isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}
	
		if(getTriggerActionInCollider().IsTouching(collider)) {

			if(!loot.canCollect()) {
				PlayerActionsManager.Instance.showAction(new ActionLootCollect(this, false));
			} else if(loot.mustReorderBeforeCollecting()) {
				PlayerActionsManager.Instance.showAction(new ActionLootCollectThenSendToHub(this));
			} else {
				PlayerActionsManager.Instance.showAction(new ActionLootCollect(this, true));
			}

			isColliding = PlayerActionsManager.Instance.isShowingAction(this);
		}

	}

	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(!isColliding) {
			return;
		}

		if(loot.isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}
		
		if(!getTriggerActionOutCollider().IsTouching(collider)) {

			isColliding = false;

			PlayerActionsManager.Instance.hideAction(new ActionLootCollectThenSendToHub(this));
			PlayerActionsManager.Instance.hideAction(new ActionLootCollect(this, false));
		}

	}


	void IActionnable.notifyActionShown(BaseAction action) {

		//show grid

		GameObject gridObject = GameObject.Find(Constants.GAME_OBJECT_NAME_GRID_COLLECTIBLE_ITEM);

		Image gridImage = gridObject.GetComponent<Image>();
		gridImage.enabled = true;

		InventoryGrid grid = gridObject.GetComponent<InventoryGrid>();
		grid.width = loot.itemPattern.widthInBlocks;
		grid.maxWidth = loot.itemPattern.widthInBlocks;
		grid.height = loot.itemPattern.heightInBlocks;
		grid.maxHeight = loot.itemPattern.heightInBlocks;

		grid.deleteGridViews();
		grid.updateGridViews();

		ItemInGrid item = new ItemInGrid(loot.itemPattern, grid.name);

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

		if(loot.isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		if(!loot.canCollect()) {
			return;
		}

		StartCoroutine(delayPlayerAfterAction());

	}

	
	private IEnumerator delayPlayerAfterAction() {
		
		PlayerBehavior playerBehavior = GameHelper.Instance.findPlayerBehavior();
		
		playerBehavior.disableControls();
		getTriggerActionInCollider().enabled = false;

		
		yield return new WaitForSeconds(0.75f);

		startCollecting();
		
		playerBehavior.enableControls();

	}


}

