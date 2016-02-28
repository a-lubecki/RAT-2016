using System;
using System.Collections.Generic;

public class ItemsManager {

	
	private static ItemsManager instance;
	
	private ItemsManager() {}
	
	public static ItemsManager Instance {
		
		get {
			if (instance == null) {
				instance = new ItemsManager();
				instance.loadAllItems();
			}
			return instance;
		}
	}


	private Dictionary<string, Item> itemsById = new Dictionary<string, Item>();


	private void loadAllItems() {

		//load all items from file
		
		//TODO
	}

	public Item findItem(string itemId) {
		return itemsById[itemId];
	}

}

