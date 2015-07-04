using UnityEngine;
using System.Collections;
using Level;

public class Player : MonoBehaviour {

	public int life { get; private set; }

	public void init() {
		
		this.life = 10;
	}

}

