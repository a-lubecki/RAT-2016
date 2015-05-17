using UnityEngine;
using System.Collections;

public class CameraResizer : MonoBehaviour {
	
	public int nbTilesInCamHeight = 1;
	private int lastNbTilesInCamHeight = 0;
	private int lastHeightMultiplier = 0;

	void Start () {

		Update();
	}

	void Update () {
		/*
		if(nbTilesInCamHeight <= 0) {
			nbTilesInCamHeight = 1;
		}

		int tilesHeightToDisplay = nbTilesInCamHeight * Constants.TILE_SIZE;

		int heightMultiplier = Screen.height / tilesHeightToDisplay;//no float
		if(heightMultiplier < 1) {
			heightMultiplier = 1;
		}

		if(heightMultiplier != lastHeightMultiplier || 
		   nbTilesInCamHeight != lastNbTilesInCamHeight) {//update

			lastHeightMultiplier = heightMultiplier;
			lastNbTilesInCamHeight = nbTilesInCamHeight;

			Camera cam = GetComponent<Camera>();
			
			float newSize = heightMultiplier * nbTilesInCamHeight * 0.5f + 
				((float)(Screen.height / (float)tilesHeightToDisplay) - heightMultiplier) * 0.5f;

			cam.orthographicSize = newSize; //TODO animate resizing

		}
*/

	}
}
