using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInGridBehavior : MonoBehaviour {

	public ItemInGrid itemInGrid { get; private set; }


	public void init(ItemInGrid itemInGrid) {

		if(itemInGrid == null) {
			throw new ArgumentException();
		}

		this.itemInGrid = itemInGrid;

		if(isActiveAndEnabled) {
			itemInGrid.addBehavior(this);
		}
	}

	void OnEnable() {

		if(itemInGrid == null) {
			return;
		}

		itemInGrid.addBehavior(this);
	}

	void OnDisable() {

		if(itemInGrid == null) {
			return;
		}

		itemInGrid.removeBehavior(this);
	}


	public void updateViews() {

		float scale = 0.8f;

		ItemPattern itemPattern = itemInGrid.getItemPattern();

		RectTransform itemRectTransform = GetComponent<RectTransform>();
		itemRectTransform.sizeDelta = new Vector2(itemPattern.widthInBlocks, itemPattern.heightInBlocks);

		RectTransform parentTransform =  transform.parent.GetComponent<RectTransform>();
		itemRectTransform.pivot = parentTransform.pivot;

		int yBlocksRotation = 0;
		if(itemInGrid.getOrientation() == Orientation.FACE) {
			itemRectTransform.localRotation = Quaternion.identity;
		} else {
			itemRectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
			yBlocksRotation = itemPattern.widthInBlocks;
		}

		itemRectTransform.localPosition = new Vector3(itemInGrid.getPosXInBlocks() * scale, - (itemInGrid.getPosYInBlocks() + yBlocksRotation) * scale, 0);
		itemRectTransform.localScale = new Vector3(scale, scale, 1);

		Image itemImage = GetComponent<Image>();
		itemImage.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_ITEMS + itemPattern.imageName);

		//add nb grouped
		int nbGrouped = itemInGrid.getNbGrouped();
		if(nbGrouped > 1) {

			Image backgroundNbGrouped = null;
			Text textNbGrouped = null;

			for(int i = 0 ; i < transform.childCount ; i++) {
				
				GameObject child = transform.GetChild(i).gameObject;

				backgroundNbGrouped = child.GetComponent<Image>();
				if(backgroundNbGrouped != null) {
					continue;
				}

				textNbGrouped = child.GetComponent<Text>();
				if(textNbGrouped != null) {
					continue;
				}
			}

			//add child
			if(backgroundNbGrouped == null) {

				GameObject gameObjectBackgroundNbGrouped = new GameObject("ItemInGridNbGrouped", new Type[]{typeof(Image)});
				gameObjectBackgroundNbGrouped.transform.SetParent(transform);
				backgroundNbGrouped = gameObjectBackgroundNbGrouped.GetComponent<Image>();

				backgroundNbGrouped.color = new Color(0, 0, 0, 0.75f);
			}

			if(textNbGrouped == null) {

				GameObject gameObjectTextNbGrouped = GameHelper.Instance.newGameObjectFromPrefab(GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_ITEMINGRID_NB_GROUPED));
				gameObjectTextNbGrouped.transform.SetParent(transform);
				textNbGrouped = gameObjectTextNbGrouped.GetComponent<Text>();

				textNbGrouped.color = Color.yellow;
			}

			textNbGrouped.text = "" + nbGrouped;

			RectTransform transformBackgroundNbGrouped = backgroundNbGrouped.GetComponent<RectTransform>();
			transformBackgroundNbGrouped.localPosition = itemRectTransform.localPosition;
			transformBackgroundNbGrouped.localScale = new Vector3(0.8f, 0.8f, 1f);
			transformBackgroundNbGrouped.pivot = new Vector2(1, 0);
			transformBackgroundNbGrouped.anchorMin = new Vector2(1, 0);
			transformBackgroundNbGrouped.anchorMax = new Vector2(1, 0);
			transformBackgroundNbGrouped.sizeDelta = new Vector2(1, 1);

			RectTransform transformTextNbGrouped = textNbGrouped.GetComponent<RectTransform>();
			transformTextNbGrouped.localPosition = itemRectTransform.localPosition;
			transformTextNbGrouped.localScale = new Vector3(0.6f, 0.6f, 1);

			if(itemInGrid.getOrientation() == Orientation.FACE) {
				transformBackgroundNbGrouped.anchoredPosition = new Vector2();
				transformTextNbGrouped.anchoredPosition = new Vector2(0.2f, 0.1f);
			} else {
				transformBackgroundNbGrouped.anchoredPosition = new Vector2(-itemPattern.widthInBlocks, 0);
				transformTextNbGrouped.anchoredPosition = new Vector2(-itemPattern.widthInBlocks + 0.1f, -0.2f);
			}

		}

	}


	public void updatePosition() {

	}

	public void updateRotation() {

	}

	public void updateVisibility() {

	}

	public void updateNbGrouped() {

	}


	public void onSelect() {

		GameHelper.Instance.getMenu().getCurrentSubMenuType().onItemSelected(itemInGrid);
	}

	public void onDeselect() {

		GameHelper.Instance.getMenu().getCurrentSubMenuType().onItemDeselected();
	}

	public void onSelectionValidated() {

		//display glassview on top
		GameObject glassGameObject = GameHelper.Instance.getForegroundGlassGameObject();

		Image image = glassGameObject.GetComponent<Image>();
		if(image.enabled) {
			//already visible
			return;
		}

		image.enabled = true;
		glassGameObject.transform.SetAsLastSibling();

		GameHelper.Instance.getItemInGridBehavior(itemInGrid).transform.SetParent(glassGameObject.transform);
		GameHelper.Instance.getMenuCursorBehavior().transform.SetParent(glassGameObject.transform);

		List<BaseAction> itemActions = new List<BaseAction>();
		itemActions.Add(new ActionItemInGridMove(itemInGrid));
		itemActions.Add(new ActionItemInGridSendToHub(itemInGrid, itemInGrid.getItemPattern().isCastable));
		itemActions.Add(new ActionItemInGridCast(itemInGrid, itemInGrid.getItemPattern().isCastable));
		ItemInGridActionsManager.Instance.showActions(itemActions);

	}

	public void onSelectionCancelled() {

		//hide glassview
		hideSelection();
	}

	private void hideSelection() {

		GameObject glassGameObject = GameHelper.Instance.getForegroundGlassGameObject();

		Image image = glassGameObject.GetComponent<Image>();
		if(!image.enabled) {
			//already hidden
			return;
		}

		image.enabled = false;

		ItemInGridBehavior itemInGridBehavior = glassGameObject.transform.GetComponentInChildren<ItemInGridBehavior>();

		InventoryGrid grid = GameHelper.Instance.getMenu().getCurrentSubMenuType().findInventoryGrid(itemInGrid.getGridName());
		itemInGridBehavior.transform.SetParent(grid.transform);

		MenuCursorBehavior menuCursorBehavior = glassGameObject.transform.GetComponentInChildren<MenuCursorBehavior>();
		menuCursorBehavior.transform.SetParent(GameHelper.Instance.getMenu().transform);

		ItemInGridActionsManager.Instance.hideActions();
	}

	public void notifyActionShown(BaseAction action) {
		//do nothing
	}

	public void notifyActionHidden(BaseAction action) {
		//do nothing
	}

	public void notifyActionValidated(BaseAction action) {

		if(action is ActionItemInGridMove) {

			//TODO;

		} else if(action is ActionItemInGridSendToHub) {


		} else if(action is ActionItemInGridCast) {

			hideSelection();

			//TODO;


			GameManager.Instance.getInventory().removeItem(itemInGrid);

			GameHelper.Instance.getMenu().getCurrentSubMenuType().findInventoryGrid(itemInGrid.getGridName()).removeItem(itemInGrid);

			GameHelper.Instance.getMenu().getCurrentSubMenuType().selectFirstItem(true);
		}


	}

}

