using System;
using Node;
using UnityEngine;

public class NoteCreator : BaseEntityCreator {


	protected override GameObject getPrefab() {
		return GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_NOTE);
	}

	protected override string getGameObjectName() {
		return Constants.GAME_OBJECT_NAME_NOTE;
	}

	protected override string getSortingLayerName() {
		return Constants.SORTING_LAYER_NAME_OBJECTS;
	}

	public GameObject createNewGameObject(NodeElementNote nodeElement, Note note) {

		if(nodeElement == null) {
			throw new System.ArgumentException();
		}
		if(note == null) {
			throw new System.ArgumentException();
		}

		GameObject gameObject = createNewGameObject(
			nodeElement.nodePosition.x, 
			nodeElement.nodePosition.y,
			Quaternion.identity, 
			GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_ENVIRONMENTS + note.imageKeyName),
			0
		);

		NoteBehavior noteBehavior = gameObject.GetComponent<NoteBehavior>();
		noteBehavior.init(note);

		return gameObject;
	}

}

