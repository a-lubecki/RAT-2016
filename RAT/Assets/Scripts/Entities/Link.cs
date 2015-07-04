using UnityEngine;
using System.Collections;
using Level;

public class Link : MonoBehaviour {

	public NodeElementLink nodeElementLink;
	
	void OnTriggerEnter2D(Collider2D other) {

		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {
			GameHelper.Instance.getLevelManager().processLink(this);
		}

	} 

}
