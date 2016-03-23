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

		float scale = 0.8f;

		ItemPattern itemPattern = itemInGrid.getItemPattern();

		RectTransform itemRectTransform = GetComponent<RectTransform>();
		itemRectTransform.sizeDelta = new Vector2(itemPattern.widthInBlocks, itemPattern.heightInBlocks);

		RectTransform parentTransform =  transform.parent.GetComponent<RectTransform>();
		itemRectTransform.pivot = parentTransform.pivot;

		itemRectTransform.localPosition = new Vector3(itemInGrid.getPosXInBlocks() * scale, - itemInGrid.getPosYInBlocks() * scale, 0);
		itemRectTransform.localScale = new Vector3(scale, scale, 1);

		Image itemImage = GetComponent<Image>();
		itemImage.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_ITEMS + itemPattern.imageName);

	}

}

