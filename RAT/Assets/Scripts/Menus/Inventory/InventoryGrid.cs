using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

	private List<ItemInGrid> items = new List<ItemInGrid>();

	/*
	public InventoryGrid siblingTop;
	public InventoryGrid siblingBottom;
	public InventoryGrid siblingLeft;
	public InventoryGrid siblingRight;
*/
	
	private static Sprite spriteInventoryPointOutside;
	private static Sprite spriteInventoryPointInside;
	private static Sprite spriteInventorySegmentOutside;
	private static Sprite spriteInventorySegmentInside;


	private string getGameObjectName(int x, int y, bool isPoint, bool isHorizontalSegment) {

		return "GridPart." +  
			(isPoint ? "Point" : ("Segment." + 
			 (isHorizontalSegment ? "H" : "V"))) +
				"." + x + "." + y;
	}

	public List<ItemInGrid> getItems() {
		return items;
	}

	public void updateViews() {

		bool mustCreateParts = (transform.childCount <= 0);
		
		RectTransform rt = GetComponent<RectTransform>();

		if(mustCreateParts) {
			
			spriteInventoryPointOutside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Point.Outside");
			spriteInventoryPointInside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Point.Inside");
			spriteInventorySegmentOutside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Segment.Outside");
			spriteInventorySegmentInside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Segment.Inside");
			
			titleText.text = Constants.tr(nameTrKey);

			rt.sizeDelta = new Vector2(maxWidth * 0.8f, maxHeight * 0.8f);
			rt.localScale = new Vector3(TILE_SIZE, TILE_SIZE, 1);
		}

		for(int y = 0 ; y < maxHeight + 1 ; y++) {
			for(int x = 0 ; x < maxWidth + 1 ; x++) {
				
				//horizontal segment
				if(x < maxWidth) {
					if(mustCreateParts) {
						createGridPart(rt, x, y, false, true);
					}
					updateGridPart(rt, x, y, false, true);
				}
				//vertical segment
				if(y < maxHeight) {
					if(mustCreateParts) {
						createGridPart(rt, x, y, false, false);
					}
					updateGridPart(rt, x, y, false, false);
				}
			}
		}
		
		for(int y = 0 ; y < maxHeight + 1 ; y++) {
			for(int x = 0 ; x < maxWidth + 1 ; x++) {
				
				//point
				if(mustCreateParts) {
					createGridPart(rt, x, y, true, false);
				}
				updateGridPart(rt, x, y, true, false);
			}
		}
	}
	
	private void createGridPart(RectTransform parent, int x, int y, bool isPoint, bool isHorizontalSegment) {

		GameObject go = new GameObject(getGameObjectName(x, y, isPoint, isHorizontalSegment));
		Image im = go.AddComponent<Image>();

		im.preserveAspect = true;

		
		RectTransform rt = go.GetComponent<RectTransform>();
		rt.SetParent(parent);
		rt.anchorMin = new Vector2(0, 1);
		rt.anchorMax = new Vector2(0, 1);
		rt.localScale = new Vector3(1, 1, 1);
		
		if(isPoint) {
			
			rt.anchoredPosition = new Vector2(x * 0.8f, -y * 0.8f);
			rt.sizeDelta = new Vector2(0.2f, 0.2f);
			rt.pivot = new Vector2(0.5f, 0.5f);
			
		} else {
			
			rt.sizeDelta = new Vector2(1, 0.1f);
			rt.pivot = new Vector2(0, 0.5f);
			
			if(!isHorizontalSegment) {
				rt.anchoredPosition = new Vector2(x * 0.8f, -y * 0.8f + 0.05f);
				rt.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
			} else {
				rt.anchoredPosition = new Vector2(x * 0.8f - 0.05f, -y * 0.8f);
			}
		}

	}

	private void updateGridPart(RectTransform parent, int x, int y, bool isPoint, bool isHorizontalSegment) {

		GameObject go = parent.Find(getGameObjectName(x, y, isPoint, isHorizontalSegment)).gameObject;
		Image im = go.GetComponent<Image>();

		Sprite sprite;
		Color color = Color.white;
		if(isPoint) {

			if((x <= 0 || x >= maxWidth) || (y <= 0 || y >= maxHeight)) {
				sprite = spriteInventoryPointOutside;
			} else {
				sprite = spriteInventoryPointInside;
			}

			if(x > width || y > height) {
				color = Color.grey;
			}

		} else {

			if(((x <= 0 || x >= maxWidth) && !isHorizontalSegment) ||
			   ((y <= 0 || y >= maxHeight) && isHorizontalSegment)) {
				sprite = spriteInventorySegmentOutside;
			} else {
				sprite = spriteInventorySegmentInside;
			}

			if((x > width && !isHorizontalSegment) ||
			   (x > width - 1 && isHorizontalSegment) ||
			   (y > height && isHorizontalSegment) ||
			   (y > height - 1 && !isHorizontalSegment)) {
				color = Color.grey;
			}
		}

		im.sprite = sprite;

		im.color = color;

	}

	
	public void updateItems(List<ItemInGrid> items) {

		this.items = new List<ItemInGrid>(items);

		//TODO update items

	}
	
	public void addItem(ItemInGrid item) {

		//TODO

	}
	
	public void removeItem(ItemInGrid item) {
		
		//TODO
	}
	
	public void moveItem(ItemInGrid item) {
		
		//TODO
	}


}

