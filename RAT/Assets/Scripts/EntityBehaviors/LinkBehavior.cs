using UnityEngine;
using System;
using Node;

public class LinkBehavior : BaseEntityBehavior {

	public Link link {
		get {
			return (Link) entity;
		}
	}

	public void init(Link link) {

		base.init(link);

	}

	void OnTriggerEnter2D(Collider2D other) {

		if(Constants.GAME_OBJECT_NAME_PLAYER.Equals(other.name)) {
			GameHelper.Instance.getLevelManager().processLink(link);
		}

	} 

}
