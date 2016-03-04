using System;
using System.Collections.Generic;

[Serializable]
public class InventorySaveData {
	
	protected Dictionary<string, ItemInGridListSaveData> itemsDataByGridName = new Dictionary<string, ItemInGridListSaveData>();
	
	public InventorySaveData(InventoryGrid[] grids) {
		
		if(grids == null) {
			return;
		}

		foreach(InventoryGrid grid in grids) {

			ItemInGridListSaveData itemListData = new ItemInGridListSaveData(grid.getItems());
			itemsDataByGridName.Add(grid.name, itemListData);
		}

	}
	
	public Dictionary<string, ItemInGridListSaveData> getItemsDataByGridName() {
		return new Dictionary<string, ItemInGridListSaveData>(itemsDataByGridName);
	}

}

