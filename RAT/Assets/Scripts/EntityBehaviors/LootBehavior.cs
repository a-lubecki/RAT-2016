using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Node;

public class LootBehavior : MonoBehaviour, IActionnable {

	public Loot loot { get; private set; }

	public bool isCollecting { get; private set; }

	private bool isColliding = false;

	public void init(Loot loot) {

		if(loot == null) {
			throw new ArgumentException();
		}

		this.loot = loot;

		GetComponent<Gif>().startAnimation();

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

		if(loot.mustReorderBeforePickingUp()) {
			
			menu.open(Constants.MENU_TYPE_INVENTORY);
			
			//TODO call endCollecting(true/false) when the menu is closed

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
		
		loot.setCollected();
		
		getTriggerActionInCollider().enabled = false;
		
		GetComponent<Gif>().stopAnimation();

		//hide the image, can't disable and destroy the object because it won't be saved with the collected items
		GetComponent<SpriteRenderer>().sprite = null;

		//add to inventory
		ItemInGrid item = new ItemInGrid();

		string gridName = loot.itemPattern.getFirstGridName();

		int[] itemCoords = Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT.getNewItemCoords(gridName, loot.itemPattern);
		if(itemCoords == null) {
			throw new NotSupportedException("TODO manage groupable items");
		}
		item.init(loot.itemPattern, gridName, itemCoords[0], itemCoords[1], loot.nbGrouped);

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

			isColliding = true;

			if(loot.mustReorderBeforePickingUp()) {
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
		grid.width = loot.itemPattern.widthInBlocks;
		grid.maxWidth = loot.itemPattern.widthInBlocks;
		grid.height = loot.itemPattern.heightInBlocks;
		grid.maxHeight = loot.itemPattern.heightInBlocks;

		grid.deleteGridViews();
		grid.updateGridViews();

		ItemInGrid item = new ItemInGrid();
		item.init(loot.itemPattern, grid.name, 0, 0, loot.nbGrouped);

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

