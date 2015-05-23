using UnityEngine; 
using System.Collections; 

public abstract class EntityCollider : MonoBehaviour {

	public float angleDegrees { get; private set; }
	public bool isMoving { get; private set; }
	
	protected bool isPaused { get; private set; }

	void FixedUpdate() {

		if(isPaused) {
			return;
		}

		Vector2 newVector = getNewMoveVector();

		//update infos
		float dx = newVector.x;
		float dy = newVector.y;

		isMoving = (dx != 0 || dy != 0);

		if(isMoving) {

			angleDegrees = vectorToAngle(newVector.x, newVector.y);

			Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

			//update transform with int vector to move with the grid
			rigidBody.MovePosition(
				new Vector2(
					rigidBody.position.x + dx * Time.deltaTime, 
					rigidBody.position.y + dy * Time.deltaTime
				)
			);
		}

	}

	protected void OnApplicationFocus(bool focusStatus) {
		this.isPaused = !focusStatus;
	}


	protected abstract Vector2 getNewMoveVector();

	public static float vectorToAngle(float x, float y) {
		
		if(y == 0) {
			return (x > 0) ? 90 : -90;
		}
		
		return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
	}

}

