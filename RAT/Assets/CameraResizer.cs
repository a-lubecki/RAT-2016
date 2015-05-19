using UnityEngine;
using System.Collections;

// http://docs.unity3d.com/Manual/class-Camera.html
/**
 * The CameraResizer adapt the screen to fit several tiles (nbVisibleTiles) in the screen height regardless of the screen size.
 * The tile size will be scaled by steps (32 > 64 > 96 > ...) and the screen viewport (only height) will be resized to avoid glitches.
 * If the tile size reaches the minimum (32px) the size won't be reduced and the nbVisibleTiles field may be ignored.
 */
public class CameraResizer : MonoBehaviour {
	
	[Tooltip("Nb min of visible tiles in the screen, may be ignored if too big, can't be 0")]
	public int nbVisibleTiles;

	void Start () {
		
		scaleScene();
	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.F)) {
			Screen.fullScreen = !Screen.fullScreen;//TEST with fullscreen
		}

		scaleScene();
	}

	private void scaleScene() {

		if(nbVisibleTiles <= 0) {
			throw new System.InvalidOperationException();
		}


		int minHeightToDisplay = nbVisibleTiles * Constants.TILE_SIZE;

		float ratio = Screen.height / (float)minHeightToDisplay;

		int multiplier = (int)Mathf.Floor(ratio);
		if(multiplier <= 0) {
			multiplier = 1;
		}

		int newScreenHeight = Screen.height;

		int divider = 2 * multiplier;

		int nbSparePixels = Screen.height % divider;
		if(nbSparePixels > 0) {
			newScreenHeight -= nbSparePixels;
		}
		
		
		Camera cam = GetComponent<Camera>();

		//set the viewport rect to have a pixel perfect calculation of the orthographic size,
		// it must be a multiplier of the pixel size,
		// ex : if the pixel size is 5, if the cam height is 524 => new height will be 520, spare pixels will be 4
		//spare pixels will be on top of the hud (not a big deal)
		cam.pixelRect = new Rect(0, 0, Screen.width, newScreenHeight);

		//set the orthographic size to scale the scene with the multiplier
		cam.orthographicSize = newScreenHeight / (float)divider;

		//Debug.Log(">>> " + Screen.height + " > " + newScreenHeight + " > " + cam.orthographicSize + " > " + multiplier + " > " + nbSparePixels);


	}
}