using UnityEngine;
using System.Collections;

public class CameraResizer : MonoBehaviour {
	
	public int nbTilesInCamHeight = 1;
	private int lastNbTilesInCamHeight = 0;

	void Start () {

		Update();
	}

	void Update () {

		if(nbTilesInCamHeight <= 0) {
			nbTilesInCamHeight = 1;
		}

		if(lastNbTilesInCamHeight != nbTilesInCamHeight) {

			lastNbTilesInCamHeight = nbTilesInCamHeight;

			Camera cam = GetComponent<Camera>();
			
			float newSize = nbTilesInCamHeight * 0.5f;

			cam.orthographicSize = newSize; //TODO animate resizing

		}


	}
}
