using System;

[Serializable]
public class ItemInGridSaveData {

	protected string id;

	protected int posXInBlocks;
	protected int posYInBlocks;
	
	protected int nbGrouped;
	
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

		ItemPattern itemPattern = ItemsManager.Instance.findItem(id);
		if(itemPattern == null) {
			//not found
			return;
		}

		itemInGrid.init(itemPattern, grid.name, posXInBlocks, posYInBlocks, nbGrouped);
	}
}
