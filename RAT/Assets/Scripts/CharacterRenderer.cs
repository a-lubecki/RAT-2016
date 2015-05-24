using UnityEngine;
using System.Collections;

public class CharacterRenderer : MonoBehaviour {

	public EntityCollider entityCollider;


	void FixedUpdate () {
	
		if(entityCollider == null) {
			throw new System.InvalidOperationException();
		}

		transform.position = snapToGrid(entityCollider.transform.position);

		//Debug.Log(">>> " + transform.position.x + " - " + transform.position.y);
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

}
