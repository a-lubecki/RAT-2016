using System;
using Level;
using UnityEngine;

public class NpcCreator : BaseEntityCreator {
	

	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC_COLLIDER);
	}

	protected override string getGameObjectName() {
		return Constants.PREFAB_NAME_NPC_COLLIDER;
	}
	
	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_OBJECTS;
	}

	public GameObject createNewGameObject(NodeElementNpc nodeElement) {
		
		if(nodeElement == null) {
			throw new System.ArgumentException();
		}

		int x = nodeElement.nodePosition.x;
		int y = nodeElement.nodePosition.y;

		GameObject gameObjectCollider = createNewGameObject(
			x, 
			y);

		GameObject gameObjectRenderer = GameHelper.Instance.newGameObjectFromPrefab(
			GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NPC_RENDERER), 
			x, 
			y); 
		gameObjectRenderer.name = Constants.PREFAB_NAME_NPC_RENDERER;

		EntityCollider npcCollider = gameObjectCollider.GetComponent<EntityCollider>();
		EntityRenderer npcRenderer = gameObjectRenderer.GetComponent<EntityRenderer>();
		npcRenderer.entityCollider = npcCollider;

		//TODO create npc object
		SpriteRenderer spriteRenderer = npcRenderer.GetComponent<SpriteRenderer>();
		Texture2D texture = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_CHARACTERS + "Enemy.Insect.png");
		spriteRenderer.sprite = Sprite.Create(
			texture, 
			new Rect(0, 0, Constants.TILE_SIZE, Constants.TILE_SIZE),
			new Vector2(0.5f, 0.5f),
			Constants.TILE_SIZE);


		Npc npc = gameObjectCollider.GetComponent<Npc>();
		npc.init();

		return gameObjectCollider;
	}

}

