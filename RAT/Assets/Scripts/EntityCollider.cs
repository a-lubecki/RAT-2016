using UnityEngine; 
using System.Collections; 
using Level;

public abstract class EntityCollider : MonoBehaviour {

	public float angleDegrees { get; private set; }
	public bool isMoving { get; private set; }
	
	protected bool isPaused { get; private set; }


	public void setPosition(int xGeneration, int yGeneration) {

		GetComponent<Transform>().position = new Vector2(
			xGeneration * Constants.TILE_SIZE, 
			- yGeneration * Constants.TILE_SIZE);
	}

	public void setDirection(NodeDirection.Direction direction) {

		if(direction == NodeDirection.Direction.UP) {
			angleDegrees = 0;
		} else if(direction == NodeDirection.Direction.RIGHT) {
			angleDegrees = 90;
		} else if(direction == NodeDirection.Direction.DOWN) {
			angleDegrees = 180;
		} else if(direction == NodeDirection.Direction.LEFT) {
			angleDegrees = -90;
		}
 	}

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

