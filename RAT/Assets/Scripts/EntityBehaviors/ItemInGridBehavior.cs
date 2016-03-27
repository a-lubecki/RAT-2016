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
			transformBackgroundNbGrouped.anchoredPosition = new Vector2();

			RectTransform transformTextNbGrouped = textNbGrouped.GetComponent<RectTransform>();
			transformTextNbGrouped.localPosition = itemRectTransform.localPosition;
			transformTextNbGrouped.localScale = new Vector3(0.6f, 0.6f, 1);
			transformTextNbGrouped.anchoredPosition = new Vector2(0.2f, 0.1f);

		}

	}

}

