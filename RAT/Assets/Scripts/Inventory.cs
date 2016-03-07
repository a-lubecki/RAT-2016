using System;
using System.Collections.Generic;

public class Inventory {

	private HashSet<ItemInGrid> items = new HashSet<ItemInGrid>();//TODO improve search with dictionaries


	public List<ItemInGrid> getItems() {
		return new List<ItemInGrid>(items);
	}

	public ItemInGrid getItem(string gridName, int posXInGrid, int posYInGrid) {

		if(string.IsNullOrEmpty(gridName)) {
			throw new ArgumentException();
		}

		foreach(ItemInGrid item in items) {
			if(gridName.Equals(item.getGridName()) && posXInGrid == item.getPosXInBlocks() && posYInGrid == item.getPosYInBlocks()) {
				return item;
			}
		}

		return null;
	}

	public void addItem(ItemInGrid newItem) {

		if(newItem == null) {
			throw new ArgumentException();
		}

		items.Add(newItem);
	}


	public void removeItem(ItemInGrid item) {

		if(item == null) {
			throw new ArgumentException();
		}

		items.Remove(item);
	}


	public bool hasItemWithPattern(ItemPattern itemPattern) {

		if(itemPattern == null) {
			throw new ArgumentException();
		}

		foreach(ItemInGrid item in items) {
			if(itemPattern == item.getItem()) {
				return true;
			}
		}

		return false;
	}


}

