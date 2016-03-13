using System;

[Serializable]
public class ItemInGridSaveData {

	protected string itemPatternId;

	protected string gridName;
	protected int posXInBlocks;
	protected int posYInBlocks;
	
	protected int nbGrouped;
	
	public string getItemPatternId() {
		return itemPatternId;
	}
	public string getGridName() {
		return gridName;
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

		itemPatternId = itemInGrid.getItemPattern().id;

		gridName = itemInGrid.getGridName();
		posXInBlocks = itemInGrid.getPosXInBlocks();
		posYInBlocks = itemInGrid.getPosYInBlocks();

		nbGrouped = itemInGrid.getNbGrouped();
	}
	
	public void assign(ItemInGrid itemInGrid) {

		if(itemInGrid == null) {
			throw new System.ArgumentException();
		}

		ItemPattern itemPattern = GameManager.Instance.getNodeGame().findItemPattern(itemPatternId);
		if(itemPattern == null) {
			//not found
			return;
		}

		itemInGrid.init(itemPattern, gridName, posXInBlocks, posYInBlocks, nbGrouped);
	}
}
