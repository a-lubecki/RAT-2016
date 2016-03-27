using System;

[Serializable]
public class ItemInGridSaveData {

	protected string itemPatternId;

	protected string gridName;
	protected int posXInBlocks;
	protected int posYInBlocks;
	protected Orientation orientation;
	
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
	public Orientation getOrientation() {
		return orientation;
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
		orientation = itemInGrid.getOrientation();

		nbGrouped = itemInGrid.getNbGrouped();
	}
	

}
