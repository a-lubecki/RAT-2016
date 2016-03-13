using UnityEngine;
using System.Collections;
using Node;

public class Link : MonoBehaviour {

	public NodeElementLink nodeElementLink;
	
	void OnTriggerEnter2D(Collider2D other) {

		if(Constants.GAME_OBJECT_NAME_PLAYER.Equals(other.name)) {
			GameHelper.Instance.getLevelManager().processLink(this);
		}

	} 

}
