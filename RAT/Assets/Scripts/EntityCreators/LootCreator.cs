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

	public GameObject createNewGameObject(NodeElementLoot nodeElement, Loot loot) {
		
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

		LootBehavior lootBehavior = gameObject.GetComponent<LootBehavior>();
		lootBehavior.init(loot);

		return gameObject;
	}

}

