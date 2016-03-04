using System;
using UnityEngine;
using Node;

public abstract class BaseEntityCreator {

	private GameObject prefab;
	private string gameObjectName;
	private Transform parentTransform;

	private string sortingLayerName;

	private Sprite debugSprite;

	
	private GameObject getPrefabInternal() {
		
		if(prefab == null) {
			
			prefab = getPrefab();
			
			if(prefab == null) {
				throw new System.InvalidOperationException();
			}
		}
		
		return prefab;
	}

	private string getGameObjectNameInternal() {
		
		if(gameObjectName == null) {
			
			gameObjectName = getGameObjectName();
			
			if(string.IsNullOrEmpty(gameObjectName)) {
				throw new System.InvalidOperationException();
			}
		}
		
		return gameObjectName;
	}

	private Transform getParentTransformInternal() {

		if(parentTransform == null) {

			parentTransform = getParentTransform();

			if(parentTransform == null) {
				throw new System.InvalidOperationException();
			}
		}

		return parentTransform;
	}
	
	private string getSortingLayerNameInternal() {
		
		if(sortingLayerName == null) {
			sortingLayerName = getSortingLayerName();
		}
		
		return sortingLayerName;
	}

	private Sprite getDebugSpriteInternal() {

		if(debugSprite == null) {
			debugSprite = getDebugSprite();
		}
		
		return debugSprite;
	}


	protected GameObject createNewGameObject(int x, int y) {
		return createNewGameObject(x, y, Quaternion.identity, null, 0);
	}

	protected GameObject createNewGameObject(int x, int y, Quaternion rotation, Sprite sprite, int orderInLayer) {

		GameObject gameObject = GameHelper.Instance.newGameObjectFromPrefab(
			getPrefabInternal(), 
			x, 
			y, 
			rotation);

		gameObject.transform.SetParent(getParentTransformInternal());
		
		gameObject.name = getGameObjectNameInternal();

		if(sprite != null) {

			SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;

			string sortingLayerName = getSortingLayerNameInternal();
			if(!string.IsNullOrEmpty(sortingLayerName)) {
				spriteRenderer.sortingLayerName = sortingLayerName;
			}

			spriteRenderer.sortingOrder = orderInLayer;
		}

		//display debug image
		if(Debug.isDebugBuild) {

			Sprite debugSprite = getDebugSpriteInternal();
			if(debugSprite != null) {

				SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
				if(spriteRenderer == null) {
					spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				}
				
				spriteRenderer.sprite = debugSprite;
				spriteRenderer.sortingLayerName = Constants.SORTING_LAYER_NAME_OBJECTS;
			}
		}

		return gameObject;
	}


	protected virtual bool hasSpriteRenderer() {
		return true;
	}
	
	protected abstract GameObject getPrefab();

	protected abstract string getGameObjectName();
	
	protected virtual Transform getParentTransform() {
		return GameHelper.Instance.getMapGameObject().transform;
	}

	protected virtual string getSortingLayerName() {
		return null;
	}

	protected virtual Sprite getDebugSprite() {
		return null;
	}


}

