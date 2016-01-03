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

		//TODO create border images

		titleText.text = Constants.tr(nameTrKey);

		RectTransform rectTransform = GetComponent<RectTransform>();

		rectTransform.localScale = new Vector3(maxWidth * TILE_SIZE, maxHeight * TILE_SIZE, 1);

	}

}

