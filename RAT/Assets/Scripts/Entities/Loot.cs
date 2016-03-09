using System;
using System.Collections;
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

	public void startCollecting() {

		if(isCollected) {
			return;
		}
		if(isCollecting) {
			return;
		}

		isCollecting = true;

		bool mustReorder = false;//TODO check if must reorder after the collecting

		if(mustReorder) {
			GameHelper.Instance.getMenu().open(Constants.MENU_TYPE_INVENTORY);
			
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
		item.init(itemPattern, nbGrouped);//TODO add in grid
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

			bool mustReorder = false;//TODO check if must reorder after the collecting
			if(mustReorder) {
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

		//show retrievable item image

		GameObject imageObject = GameObject.Find(Constants.GAME_OBJECT_NAME_IMAGE_ITEM_RETRIEVABLE);
		Image imageComponent = imageObject.GetComponent<Image>();

		RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(itemPattern.widthInBlocks, itemPattern.heightInBlocks);

		rectTransform.localScale = new Vector3(1.6f, 1.6f, 1);

		imageComponent.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_ITEMS + itemPattern.imageName);

		imageComponent.enabled = true;

	}

	void IActionnable.notifyActionHidden(BaseAction action) {

		//hide retrievable item image

		GameObject imageObject = GameObject.Find(Constants.GAME_OBJECT_NAME_IMAGE_ITEM_RETRIEVABLE);
		Image imageComponent = imageObject.GetComponent<Image>();

		imageComponent.enabled = false;

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

