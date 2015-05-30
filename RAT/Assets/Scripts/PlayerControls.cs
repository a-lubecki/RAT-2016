using UnityEngine; 
using System.Collections;
using Level;


public class PlayerControls : EntityCollider { 
	
	public KeyCode[] KEYS_RIGHT = new KeyCode[] {
		KeyCode.RightArrow,
		KeyCode.D
	};
	public KeyCode[] KEYS_LEFT = new KeyCode[] {
		KeyCode.LeftArrow,
		KeyCode.Q
	};
	public KeyCode[] KEYS_UP = new KeyCode[] {
		KeyCode.UpArrow,
		KeyCode.Z
	};
	public KeyCode[] KEYS_DOWN = new KeyCode[] {
		KeyCode.DownArrow,
		KeyCode.S
	};

	public float moveSpeed = 1;
		

	protected override void Start() {

		NodeElementSpawn nodeSpawn = LevelManager.level.spawnElement;
		xGeneration = nodeSpawn.nodePosition.x;
		yGeneration = nodeSpawn.nodePosition.y;

		NodeDirection.Direction direction = nodeSpawn.nodeDirection.value;
		if(direction == NodeDirection.Direction.UP) {
			angleDegreesGeneration = 0;
		} else if(direction == NodeDirection.Direction.RIGHT) {
			angleDegreesGeneration = 90;
		} else if(direction == NodeDirection.Direction.DOWN) {
			angleDegreesGeneration = 180;
		} else if(direction == NodeDirection.Direction.LEFT) {
			angleDegreesGeneration = -90;
		}

		base.Start();
	}

	protected override Vector2 getNewMoveVector() {

		float angleDegrees = 0;
		bool isPressingAnyDirection = false;


		//TODO analogic directions here


		if(!isPressingAnyDirection) {

			int dx = 0;
			int dy = 0;

			if(isAnyKeyPressed(KEYS_RIGHT)) {
				dx += -1;
			}
			if(isAnyKeyPressed(KEYS_LEFT)) {
				dx += 1;
			}

			if(isAnyKeyPressed(KEYS_UP)) {
				dy += 1;
			}
			if(isAnyKeyPressed(KEYS_DOWN)) {
				dy += -1;
			}

			if(dx != 0 || dy != 0) {

				isPressingAnyDirection = true;

				angleDegrees = vectorToAngle(dx, dy) + 90;
			}

		}


		if(!isPressingAnyDirection) {
			return Vector2.zero;
		}

		float x = moveSpeed * Mathf.Cos(angleDegrees * Mathf.Deg2Rad);
		float y = moveSpeed * Mathf.Sin(angleDegrees * Mathf.Deg2Rad);

		return new Vector2(x, y);

	}
	
	
	private bool isKeyPressed(KeyCode key) {
		return Input.GetKey(key);
	}
	
	private bool isAnyKeyPressed(KeyCode[] keys) {

		foreach (KeyCode k in keys) {
			if(isKeyPressed(k)) {
				return true;
			}
		}

		return false;
	}
	
	private bool isAllKeyPressed(KeyCode[] keys) {
		
		foreach (KeyCode k in keys) {
			if(!isKeyPressed(k)) {
				return false;
			}
		}
		
		return true;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if(Constants.PREFAB_NAME_TILE_LINK.Equals(other.name)) {
			LevelManager.loadNextMap(this, other);
		}

		//Debug.Log("OnTriggerEnter");
	} 
	/*
	void OnTriggerExit2D(Collider2D other) {
		Debug.Log("OnTriggerExit");    
	} 

	void OnTriggerStay2D(Collider2D other) {
		Debug.Log("OnTriggerStay");    
	}*/

}

