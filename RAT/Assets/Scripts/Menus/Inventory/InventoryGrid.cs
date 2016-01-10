using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour {

	private static readonly int TILE_SIZE = 2;


	public Text titleText;

	public string nameTrKey = "";
	public string descriptionTrKey = "";
	
	public int width = 1;
	public int height = 1;

	public int maxWidth = 1;
	public int maxHeight = 1;

	public int maxItems = 0;

	public string[] typesFilterTags;
	private ItemType[] typesFilter;

	/*
	public InventoryGrid siblingTop;
	public InventoryGrid siblingBottom;
	public InventoryGrid siblingLeft;
	public InventoryGrid siblingRight;
*/

	public void build() {


		titleText.text = Constants.tr(nameTrKey);

		RectTransform rt = GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(maxWidth * 0.8f, maxHeight * 0.8f);
		rt.localScale = new Vector3(TILE_SIZE, TILE_SIZE, 1);

		for(int y = 0 ; y < maxHeight + 1 ; y++) {
			for(int x = 0 ; x < maxWidth + 1 ; x++) {

				if(x < maxWidth) {
					createImage(rt, x, y, false, true);
				}
				if(y < maxHeight) {
					createImage(rt, x, y, false, false);
				}
			}
		}
		
		for(int y = 0 ; y < maxHeight + 1 ; y++) {
			for(int x = 0 ; x < maxWidth + 1 ; x++) {

				createImage(rt, x, y, true, false);
			}
		}
	}

	private void createImage(RectTransform parent, int x, int y, bool isPoint, bool isHorizontalSegment) {

		GameObject goH = new GameObject();
		Image iH = goH.AddComponent<Image>();

		string imageName;
		Color color = Color.white;
		if(isPoint) {

			if((x <= 0 || x >= maxWidth) || (y <= 0 || y >= maxHeight)) {
				imageName = "Inventory.Point.Outside";
			} else {
				imageName = "Inventory.Point.Inside";
			}

			if(x > width || y > height) {
				color = Color.grey;
			}

		} else {

			if(((x <= 0 || x >= maxWidth) && !isHorizontalSegment) ||
			   ((y <= 0 || y >= maxHeight) && isHorizontalSegment)) {
				imageName = "Inventory.Segment.Outside";
			} else {
				imageName = "Inventory.Segment.Inside";
			}

			if((x > width && !isHorizontalSegment) ||
			   (x > width - 1 && isHorizontalSegment) ||
			   (y > height && isHorizontalSegment) ||
			   (y > height - 1 && !isHorizontalSegment)) {
				color = Color.grey;
			}
		}

		iH.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + imageName);
		iH.preserveAspect = true;

		iH.color = color;
		
		RectTransform rtH = goH.GetComponent<RectTransform>();
		rtH.SetParent(parent);
		rtH.anchorMin = new Vector2(0, 1);
		rtH.anchorMax = new Vector2(0, 1);
		rtH.localScale = new Vector3(1, 1, 1);

		if(isPoint) {
			
			rtH.anchoredPosition = new Vector2(x * 0.8f, -y * 0.8f);
			rtH.sizeDelta = new Vector2(0.2f, 0.2f);
			rtH.pivot = new Vector2(0.5f, 0.5f);
		
		} else {
			
			rtH.sizeDelta = new Vector2(1, 0.1f);
			rtH.pivot = new Vector2(0, 0.5f);

			if(!isHorizontalSegment) {
				rtH.anchoredPosition = new Vector2(x * 0.8f, -y * 0.8f + 0.05f);
				rtH.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
			} else {
				rtH.anchoredPosition = new Vector2(x * 0.8f - 0.05f, -y * 0.8f);
			}
		}

	}
	
}

