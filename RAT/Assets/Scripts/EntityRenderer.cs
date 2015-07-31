using UnityEngine;
using System.Collections;

public class EntityRenderer : MonoBehaviour {

	public EntityCollider entityCollider;

	public string currentSpritePrefix;

	void FixedUpdate () {
	
		if(entityCollider == null) {
			throw new System.InvalidOperationException();
		}

		transform.position = snapToGrid(entityCollider.transform.position);

		//Debug.Log(">>> " + transform.position.x + " - " + transform.position.y);

		//TODO update "ground" tiles around player with GameObjects pooling : http://blogs.msdn.com/b/dave_crooks_dev_blog/archive/2014/07/21/object-pooling-for-unity3d.aspx

	}

	private static Vector2 snapToGrid(Vector2 vector) {
		return new Vector2(
			snapToGrid(vector.x + Constants.PIXEL_SIZE * 0.5f), 
			snapToGrid(vector.y - Constants.PIXEL_SIZE * 0.5f));
	}
	
	private static float snapToGrid(float value) {

		float diff = value % Constants.PIXEL_SIZE; // for PIXEL_SIZE == 1, diff : 385.7 % 1 = 0.7
		return value - diff; // 385.7 - 0.7 = 385.7
	}

	public void updateSprite() {

		if(entityCollider.currentCharacterAnimation == null) {
			return;
		}

		Sprite sprite = GameHelper.Instance.loadMultiSpriteAsset(
			Constants.PATH_RES_CHARACTERS + entityCollider.currentCharacterAnimation.textureName,
			entityCollider.currentCharacterAnimationKey + "." + entityCollider.currentDirection.ToString());

		GetComponent<SpriteRenderer>().sprite = sprite;

	}
	
}
