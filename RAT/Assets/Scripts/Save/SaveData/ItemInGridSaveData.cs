using System;

[Serializable]
public class ItemInGridSaveData {

	private string id;

	private int posXInBlocks;
	private int posYInBlocks;
	
	private int nbGrouped;
	
	public string getId() {
		return id;
	}
	public int getPosXInBlocks() {
		return posXInBlocks;
	}
	public int getPosYInBlocks() {
		return posYInBlocks;
	}
	public int getNbGrouped() {
		return nbGrouped;
	}

	public ItemInGridSaveData(ItemInGrid itemInGrid) {

		if(itemInGrid == null) {
			throw new System.ArgumentException();
		}

		id = itemInGrid.getItem().id;
		
		posXInBlocks = itemInGrid.getPosXInBlocks();
		posYInBlocks = itemInGrid.getPosYInBlocks();

		nbGrouped = itemInGrid.getNbGrouped();
	}
	
	public void assign(InventoryGrid grid, ItemInGrid itemInGrid) {

		if(itemInGrid == null) {
			throw new System.ArgumentException();
		}

		itemInGrid.init(ItemsManager.Instance.findItem(id), grid.name, posXInBlocks, posYInBlocks, nbGrouped);
	}
}
