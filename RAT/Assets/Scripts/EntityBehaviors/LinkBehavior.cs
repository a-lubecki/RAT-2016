using UnityEngine;
using System;
using Node;

public class LinkBehavior : MonoBehaviour {

	public Link link { get; private set; }

	public void init(Link link) {

		if(link == null) {
			throw new ArgumentException();
		}

		this.link = link;

	}

	void OnTriggerEnter2D(Collider2D other) {

		if(Constants.GAME_OBJECT_NAME_PLAYER.Equals(other.name)) {
			GameHelper.Instance.getLevelManager().processLink(link);
		}

	} 

}
