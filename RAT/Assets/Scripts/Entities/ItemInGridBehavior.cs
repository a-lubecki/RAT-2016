﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemInGridBehavior : MonoBehaviour {

	private ItemInGrid _itemInGrid;
	public ItemInGrid itemInGrid {

		get {
			return _itemInGrid;
		}
		set {

			_itemInGrid = value;

			updateViews();

		}

	}


	public void updateViews() {

		ItemPattern itemPattern = itemInGrid.getItem();

		RectTransform itemRectTransform = GetComponent<RectTransform>();
		itemRectTransform.sizeDelta = new Vector2(itemPattern.widthInBlocks, itemPattern.heightInBlocks);
		itemRectTransform.position = new Vector3(itemInGrid.getPosXInBlocks(), itemInGrid.getPosYInBlocks(), 0);
		itemRectTransform.pivot = new Vector2(0.5f, 0);

		itemRectTransform.localPosition = new Vector3(0, 0, 0);
		itemRectTransform.localScale = new Vector3(0.8f, 0.8f, 1);

		Image itemImage = GetComponent<Image>();
		itemImage.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_ITEMS + itemPattern.imageName);

	}

}

