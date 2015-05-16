using UnityEngine; 
using System.Collections; 

public abstract class Entity : MonoBehaviour { 

	private Vector3 lastFloatPosition;
	private bool hasSetLastFloatPosition = false;

	public float angleDegrees { get; private set; }
	public bool isMoving { get; private set; }

	void FixedUpdate() {

		Vector2 newVector = getNewMoveVector();

		//update infos
		float dx = newVector.x;
		float dy = newVector.y;

		isMoving = (dx != 0 || dy != 0);

		Vector3 lastRealPosition = GetComponent<Transform>().position;

		if(isMoving) {

			angleDegrees = vectorToAngle(newVector.x, newVector.y);

			//update float vector position
			if(!hasSetLastFloatPosition) {
				lastFloatPosition = lastRealPosition;
				hasSetLastFloatPosition = true;
			} else {
				lastFloatPosition += new Vector3(dx * Time.deltaTime, dy * Time.deltaTime, 0);
			}

			//update transform with int vector to move with the grid
			GetComponent<Rigidbody2D>().MovePosition(snapToGrid(lastFloatPosition));

		} else {

			lastFloatPosition = snapToGrid(lastRealPosition);
		}

		//Debug.Log(">> " + lastFloatPosition.x + " / " + lastFloatPosition.y + " => " + isMoving);
	}


	protected abstract Vector2 getNewMoveVector();


	private static Vector2 snapToGrid(Vector2 vector) {
		return new Vector2(snapToGrid(vector.x), snapToGrid(vector.y));
	}
	
	private static float snapToGrid(float value) {

		float diff = value % Constants.PIXEL_SIZE;
		return value - diff;
	}

	public static float vectorToAngle(float x, float y) {
		
		if(y == 0) {
			return (x > 0) ? 90 : -90;
		}
		
		return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
	}

}

