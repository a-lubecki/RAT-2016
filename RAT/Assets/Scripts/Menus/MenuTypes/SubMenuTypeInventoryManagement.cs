using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuTypeInventoryManagement : AbstractSubMenuType {


	public SubMenuTypeInventoryManagement() : base("InventoryManagement") {

	}
	
	public override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_SUB_MENU_INVENTORY_MANAGEMENT;
	}

	public override List<string> getGridNames(bool topToBottom, bool leftToRight) {
		
		List<string> res = new List<string>();

		if(topToBottom) {
			
			if(leftToRight) {
				
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);

			} else {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);

			}

		} else {

			if(leftToRight) {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);

			} else {

				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_RIGHT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_EQUIP);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_WEAPONS_LEFT);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_HEALS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_OBJECTS);
				res.Add(Constants.GAME_OBJECT_NAME_GRID_BAG);
			}
		}

		return res;
	}

	public override bool isLeftGrid(string gridName) {
		return gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_BAG) ||
			gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_OBJECTS) ||
			gridName.Equals(Constants.GAME_OBJECT_NAME_GRID_HEALS);
	}


	public override void onItemSelected(ItemInGrid item) {

		base.onItemSelected(item);

		Transform transformSubMenu = getSubMenuGameObject(GameHelper.Instance.getMenu()).transform;

		Text textItemName = transformSubMenu.Find(Constants.GAME_OBJECT_NAME_MENU_ITEM_NAME).GetComponent<Text>();
		textItemName.text = item.getItemPattern().getTrName();

		Text textItemDescription = transformSubMenu.Find(Constants.GAME_OBJECT_NAME_MENU_ITEM_DESCRIPTION).GetComponent<Text>();
		textItemDescription.text = item.getItemPattern().getTrDescription();

	}

	public override void onItemDeselected() {

		base.onItemDeselected();

		Transform transformSubMenu = getSubMenuGameObject(GameHelper.Instance.getMenu()).transform;

		Text textItemName = transformSubMenu.Find(Constants.GAME_OBJECT_NAME_MENU_ITEM_NAME).GetComponent<Text>();
		textItemName.text = "";

		Text textItemDescription = transformSubMenu.Find(Constants.GAME_OBJECT_NAME_MENU_ITEM_DESCRIPTION).GetComponent<Text>();
		textItemDescription.text = "";

	}

}

