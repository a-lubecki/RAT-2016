using UnityEngine; 
using System.Collections;


public class AIControls : EntityCollider { 

	public EntityCollider playerCharacterCollider;

	public float moveSpeed = 1;
	public int xGeneration = 0;
	public int yGeneration = 0;
	
	void Start() {

		//set the first position of the player
		GetComponent<Transform>().position = new Vector2(
			xGeneration * Constants.TILE_SIZE, 
			- yGeneration * Constants.TILE_SIZE);
	}
	
	protected override Vector2 getNewMoveVector() {

		return new Vector2(0, 0);//TODO
	}

}

