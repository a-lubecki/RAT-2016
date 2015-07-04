using UnityEngine; 
using System.Collections;


public class AIControls : EntityCollider { 

	public float moveSpeed = 1;
	
	protected override Vector2 getNewMoveVector() {

		return new Vector2(0, 0);//TODO
	}

}

