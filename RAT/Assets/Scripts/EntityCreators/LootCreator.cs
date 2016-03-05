using System;
using Node;
using UnityEngine;

public class LootCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_LOOT);
	}
		
	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_LOOT;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_OBJECTS;
	}

	public GameObject createNewGameObject(NodeElementLoot nodeElement) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y,
			Quaternion.identity, 
			null,
			0
			);

		Loot loot = gameObject.GetComponent<Loot>();
		loot.setNodeElementLoot(nodeElement);

		ItemPattern itemPattern = GameManager.Instance.getNodeGame().findItemPattern(nodeElement.nodeItem.value);
		if(itemPattern == null) {
			throw new InvalidOperationException("The item pattern was not found : " + nodeElement.nodeItem.value);
		}

		loot.init(itemPattern, nodeElement.nodeNbGrouped.value);

		return gameObject;
	}

}

