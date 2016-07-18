using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Node;
using MovementEffects;

public class Loot : BaseIdentifiableModel, IActionnable {

	public ItemPattern itemPattern { get; private set; }
	public int nbGrouped { get; private set; }

	public bool isCollected { get; private set; }

	public bool isCollecting = false;
	private bool isColliding = false;

	public bool hasTriggerActionCollider { get ; private set; }


	public Loot(NodeElementLoot nodeElementLoot, bool isCollected) 
		: this(nodeElementLoot.nodeId.value, 
			BaseListenerModel.getListeners(nodeElementLoot),
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

		hasTriggerActionCollider = true;
	}


	public string getLootText() {

		string multiplier = "";
		if(nbGrouped > 1) {
			multiplier = " x" + nbGrouped;
		}
		return itemPattern.getTrName() + multiplier;
	}

	public bool canCollect() {
		
		if(itemPattern.itemType == ItemType.SPECIAL) {
			return true;
		}

		InventoryGrid grid = Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT.findInventoryGrid(itemPattern.getFirstGridName());
		if(grid.isItemPatternFitting(itemPattern)) {
			return true;
		}

		return false;
	}

	public bool mustReorderBeforeCollecting() {

		if(itemPattern.itemType == ItemType.SPECIAL) {
			return false;
		}

		InventoryGrid grid = Constants.SUB_MENU_TYPE_INVENTORY_MANAGEMENT.findInventoryGrid(itemPattern.getFirstGridName());
		if(grid.getGroupableItem(itemPattern) != null) {
			return false;
		}
		if(grid.getNewItemCoords(itemPattern) != null) {
			return false;
		}

		return true;
	}

	public void onEnterTriggerActionCollider() {

		if(isColliding) {
			return;
		}
		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		if(!canCollect()) {
			PlayerActionsManager.Instance.showAction(new ActionLootCollect(this, false));
		} else if(mustReorderBeforeCollecting()) {
			PlayerActionsManager.Instance.showAction(new ActionLootCollectThenSendToHub(this));
		} else {
			PlayerActionsManager.Instance.showAction(new ActionLootCollect(this, true));
		}

		isColliding = PlayerActionsManager.Instance.isShowingAction(this);
	}

	public void onExitTriggerActionCollider() {

		if(!isColliding) {
			return;
		}
		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		isColliding = false;

		PlayerActionsManager.Instance.hideAction(new ActionLootCollectThenSendToHub(this));
		PlayerActionsManager.Instance.hideAction(new ActionLootCollect(this, false));
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

		if(mustReorderBeforeCollecting()) {

			menu.open(Constants.MENU_TYPE_INVENTORY);

			//TODO call endCollecting(true/false) when the menu is closed

			isCollecting = false;//TODO TEST
			hasTriggerActionCollider = true;//TODO TEST
			updateBehaviors();//TODO TEST

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
			hasTriggerActionCollider = !isCollected;
			updateBehaviors();
		}

	}

	private void setCollected() {

		bool hasAddedItemInInventory = false;

		//add to inventory
		AbstractSubMenuType subMenuType = itemPattern.getFirstSubMenuType();
		string gridName = itemPattern.getFirstGridName();

		Inventory inventory = GameManager.Instance.getInventory();
		InventoryGrid inventoryGrid = subMenuType.findInventoryGrid(gridName);

		int remainingNbGrouped = nbGrouped;

		while(remainingNbGrouped > 0) {

			ItemInGrid currentGroupableItem = inventoryGrid.getGroupableItem(itemPattern);

			ItemInGrid newItemInGrid;

			if(currentGroupableItem != null) {

				newItemInGrid = currentGroupableItem.newGroupedItem(itemPattern, remainingNbGrouped);

				//replace item
				inventory.removeItem(currentGroupableItem);

			} else {

				int[] itemCoords = inventoryGrid.getNewItemCoords(itemPattern);
				if(itemCoords == null) {
					Debug.Log("Can't add new object, lack of blocks");
					break;
				}

				//split nbgrouped vs maxgroupable
				int nb = remainingNbGrouped;
				if(nb > itemPattern.maxGroupable) {
					nb = itemPattern.maxGroupable;
				}

				newItemInGrid = new ItemInGrid(itemPattern, gridName, itemCoords[0], itemCoords[1], (Orientation)itemCoords[2], nb);

			}

			inventory.addItem(newItemInGrid);

			hasAddedItemInInventory = true;

			remainingNbGrouped -= newItemInGrid.getNbGrouped();
		}


		if(hasAddedItemInInventory) {

			isCollected = true;

			updateBehaviors();
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

		ItemInGrid item = new ItemInGrid(itemPattern, grid.name);

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

		if(!canCollect()) {
			return;
		}

		Timing.RunCoroutine(delayPlayerAfterAction(), Segment.FixedUpdate);

	}


	private IEnumerator<float> delayPlayerAfterAction() {

		Player player = GameHelper.Instance.getPlayer();

		player.disableControls(this);

		hasTriggerActionCollider = false;
		updateBehaviors();

		yield return Timing.WaitForSeconds(0.75f);

		startCollecting();

		player.enableControls(this);

	}

}

