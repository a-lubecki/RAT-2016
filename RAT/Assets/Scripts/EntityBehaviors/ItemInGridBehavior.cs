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

	}

}

