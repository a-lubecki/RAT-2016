using System;
using System.Collections.Generic;

[Serializable]
public class ItemInGridListSaveData {

	protected Dictionary<string, ItemInGridSaveData> itemsDataById = new Dictionary<string, ItemInGridSaveData>();

	public ItemInGridListSaveData(List<ItemInGrid> items) {
		
		if(items == null) {
			return;
		}

		foreach(ItemInGrid item in items) {
			ItemInGridSaveData itemData = new ItemInGridSaveData(item);
			itemsDataById.Add(itemData.getId(), itemData);
		}

	}

	public Dictionary<string, ItemInGridSaveData> getItemsDataById() {
		return new Dictionary<string, ItemInGridSaveData>(itemsDataById);
	}

}

