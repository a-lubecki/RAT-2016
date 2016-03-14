using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemInGridBehavior : MonoBehaviour {

	public ItemInGrid itemInGrid { get; private set; }


	public void init(ItemInGrid itemInGrid) {

		if(itemInGrid == null) {
			throw new ArgumentException();
		}

		this.itemInGrid = itemInGrid;

		updateViews();
	}

	public void updateViews() {

		ItemPattern itemPattern = itemInGrid.getItemPattern();

		RectTransform itemRectTransform = GetComponent<RectTransform>();
		itemRectTransform.sizeDelta = new Vector2(itemPattern.widthInBlocks, itemPattern.heightInBlocks);
		itemRectTransform.position = new Vector3(itemInGrid.getPosXInBlocks(), itemInGrid.getPosYInBlocks(), 0);
		itemRectTransform.pivot = new Vector2(0.5f, 0);

		itemRectTransform.localPosition = new Vector3(itemInGrid.getPosXInBlocks(), itemInGrid.getPosYInBlocks(), 0);
		itemRectTransform.localScale = new Vector3(0.8f, 0.8f, 1);

		Image itemImage = GetComponent<Image>();
		itemImage.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_ITEMS + itemPattern.imageName);

	}

}

