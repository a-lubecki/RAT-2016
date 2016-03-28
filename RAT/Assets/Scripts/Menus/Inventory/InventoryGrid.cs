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

	public Color colorActivated = new Color(1, 1, 1, 0.75f);
	public Color colorDeactivated = new Color(0.5f, 0.5f, 0.5f, 0.75f);

	public string[] typesFilterTags;
	private ItemType[] typesFilter;

	//private List<GameObject> gridGameObjects = new List<GameObject>();
	//private List<GameObject> itemsGameObjects = new List<GameObject>();

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

	private static GameObject prefabCollectibleItem;

	private string getGameObjectName(int x, int y, bool isPoint, bool isHorizontalSegment) {

		return "GridPart." +  
			(isPoint ? "Point" : ("Segment." + 
			 (isHorizontalSegment ? "H" : "V"))) +
				"." + x + "." + y;
	}


	void Awake() {

		if(spriteInventoryPointOutside == null) {
			spriteInventoryPointOutside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Point.Outside");
		}
		if(spriteInventoryPointInside == null) {
			spriteInventoryPointInside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Point.Inside");
		}
		if(spriteInventorySegmentOutside == null) {
			spriteInventorySegmentOutside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Segment.Outside");
		}
		if(spriteInventorySegmentInside == null) {
			spriteInventorySegmentInside = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_MENUS + "Inventory.Segment.Inside");
		}
		if(prefabCollectibleItem == null) {
			prefabCollectibleItem = GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_ITEMINGRID);
		}
	}

	public void updateGridViews() {
		
		titleText.text = Constants.tr(nameTrKey);


		RectTransform rt = GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(maxWidth * 0.8f, maxHeight * 0.8f);
		rt.localScale = new Vector3(TILE_SIZE, TILE_SIZE, 1);


		for(int y = 0 ; y < maxHeight + 1 ; y++) {
			for(int x = 0 ; x < maxWidth + 1 ; x++) {
				
				//horizontal segment
				if(x < maxWidth) {
					updateGridPart(rt, x, y, false, true);
				}
				//vertical segment
				if(y < maxHeight) {
					updateGridPart(rt, x, y, false, false);
				}
			}
		}
		
		for(int y = 0 ; y < maxHeight + 1 ; y++) {
			for(int x = 0 ; x < maxWidth + 1 ; x++) {
				
				//point
				updateGridPart(rt, x, y, true, false);
			}
		}
	}

	public void deleteGridViews() {

		deleteChidrenGameObjects(false);
	}

	public void deleteChidrenGameObjects(bool deleteItemsGameObjects) {

		int count = transform.childCount;
		for(int i = count - 1 ; i >= 0 ; i--) {

			Transform childTransform = transform.GetChild(i);

			if(deleteItemsGameObjects == (childTransform.GetComponent<ItemInGridBehavior>() != null)) {
				childTransform.SetParent(null, false);
				GameObject.Destroy(childTransform.gameObject);
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

		string objectName = getGameObjectName(x, y, isPoint, isHorizontalSegment);

		Transform transform = parent.Find(objectName);
		if(transform == null) {
			createGridPart(parent, x, y, isPoint, isHorizontalSegment);
			transform = parent.Find(objectName);
		}

		Image im = transform.gameObject.GetComponent<Image>();

		Sprite sprite;
		Color color = colorActivated;
		if(isPoint) {

			if((x <= 0 || x >= maxWidth) || (y <= 0 || y >= maxHeight)) {
				sprite = spriteInventoryPointOutside;
			} else {
				sprite = spriteInventoryPointInside;
			}

			if(x > width || y > height) {
				color = colorDeactivated;
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
				color = colorDeactivated;
			}
		}

		im.sprite = sprite;

		im.color = color;

	}

	public bool isItemPatternFitting(ItemPattern itemPattern) {

		return ((itemPattern.widthInBlocks <= width && itemPattern.heightInBlocks <= height) 
			|| (itemPattern.heightInBlocks <= width && itemPattern.widthInBlocks <= height));
	}

	public ItemInGrid getGroupableItem(ItemPattern itemPattern) {
		
		if(itemPattern == null) {
			throw new ArgumentException();
		}

		Inventory inventory = GameManager.Instance.getInventory();

		//check if item pattern already exist and can be merged
		ItemInGrid itemWithPattern = inventory.getAnyItemInGridWithPattern(itemPattern);
		if(itemWithPattern != null && 
			itemWithPattern.getNbGrouped() < itemPattern.maxGroupable) {
			return itemWithPattern;
		}

		return null;
	}

	public int[] getNewItemCoords(ItemPattern itemPattern) {

		if(itemPattern == null) {
			throw new ArgumentException();
		}

		Inventory inventory = GameManager.Instance.getInventory();

		//check for taken blocks

		bool[,] takenBlocks = new bool[height, width];

		foreach(ItemInGrid item in inventory.getItems(name)) {

			int posX = item.getPosXInBlocks();
			int posY = item.getPosYInBlocks();
			int w;
			int h;
			if(item.getOrientation() == Orientation.FACE) {
				w = item.getItemPattern().widthInBlocks;
				h = item.getItemPattern().heightInBlocks;
			} else {
				w = item.getItemPattern().heightInBlocks;
				h = item.getItemPattern().widthInBlocks;
			}

			for(int y = 0 ; y < h ; y++) {

				if(y >= height) {
					break;
				}

				for(int x = 0 ; x < w ; x++) {

					if(x >= width) {
						break;
					}

					takenBlocks[posY + y, posX + x] = true;
				}
			}

		}

		//look if matches free blocks

		int wItem = itemPattern.widthInBlocks;
		int hItem = itemPattern.heightInBlocks;

		int minSize = (wItem < hItem) ? wItem : hItem;

		int wGridMinusSize = width - minSize + 1;
		int hGridMinusSize = height - minSize + 1;

		for(int j = 0 ; j < hGridMinusSize ; j++) {
			for(int i = 0 ; i < wGridMinusSize ; i++) {

				if(!takenBlocks[j, i]) {

					bool hasAllFreeBlocks = true;

					//check for non taken blocks in the itempatterns ranges horizontally
					for(int y = 0 ; y < hItem; y++) {

						if(j + y >= height) {
							hasAllFreeBlocks = false;
							break;
						}

						for(int x = 0 ; x < wItem; x++) {

							if(i + x >= width) {
								hasAllFreeBlocks = false;
								break;
							}

							if(takenBlocks[j + y, i + x]) {
								hasAllFreeBlocks = false;
								break;
							}
						}

						if(!hasAllFreeBlocks) {
							break;
						}
					}

					if(hasAllFreeBlocks) {
						return new int[] { i, j, (int) Orientation.FACE };
					}

					if(wItem == hItem) {
						//same size, no need to test with another orientation	
						break;
					}

					//reset flag
					hasAllFreeBlocks = true;

					//check for non taken blocks in the itempatterns ranges vertically
					for(int x = 0 ; x < wItem; x++) {

						if(j + x >= height) {
							hasAllFreeBlocks = false;
							break;
						}

						for(int y = 0 ; y < hItem; y++) {

							if(i + y >= width) {
								hasAllFreeBlocks = false;
								break;
							}

							if(takenBlocks[j + x, i + y]) {
								hasAllFreeBlocks = false;
								break;
							}
						}

						if(!hasAllFreeBlocks) {
							break;
						}
					}

					if(hasAllFreeBlocks) {
						return new int[] { i, j, (int)Orientation.SIDE };
					}

				}
			}
		}

		return null;
	}


	public void addItems(List<ItemInGrid> items) {

		if(items == null) {
			throw new ArgumentException();
		}

		foreach(ItemInGrid item in items) {
			addItem(item);
		}

	}

	public void addItem(ItemInGrid item) {

		if(item == null) {
			throw new ArgumentException();
		}

		//TODO check if don't exist yet

		GameObject itemObject = GameHelper.Instance.newGameObjectFromPrefab(prefabCollectibleItem);

		ItemInGridBehavior itemBehavior = itemObject.GetComponent<ItemInGridBehavior>();

		itemBehavior.transform.SetParent(transform);
		itemBehavior.init(item);

	}

	public void moveItem(ItemInGrid item) {
		
		//TODO
	}

	public void removeItem(ItemInGrid item) {

		//TODO
	}

	public void removeItems() {

		deleteChidrenGameObjects(true);
	}


}

