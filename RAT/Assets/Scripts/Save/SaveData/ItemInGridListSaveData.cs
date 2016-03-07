using System;
using System.Collections.Generic;

[Serializable]
public class ItemInGridListSaveData {

	protected List<ItemInGridSaveData> itemsInGridData = new List<ItemInGridSaveData>();

	public ItemInGridListSaveData(List<ItemInGrid> items) {
		
		if(items == null) {
			return;
		}

		foreach(ItemInGrid item in items) {
			itemsInGridData.Add(new ItemInGridSaveData(item));
		}

	}

	public List<ItemInGridSaveData> getItemsInGrid() {
		return new List<ItemInGridSaveData>(itemsInGridData);
	}

}

