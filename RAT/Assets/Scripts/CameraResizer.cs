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
	public int pixelSize { get; private set; }

	private Vector2 lastScreenSize = new Vector2();
	private int lastNbVisibleTiles = -1;
	
	void Start() {
		scaleScene();
	}

	void FixedUpdate() {
		scaleScene();
	}

	public void scaleScene() {
		
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;

		bool mustResize = (lastNbVisibleTiles != nbVisibleTiles) ||
			(screenWidth != lastScreenSize.x || screenHeight != lastScreenSize.y);

		lastNbVisibleTiles = nbVisibleTiles;
		lastScreenSize = new Vector2(screenWidth, screenHeight);

		//optimize resizing by
		if(!mustResize) {
			return;
		}

		if(nbVisibleTiles <= 0) {
			throw new System.InvalidOperationException();
		}


		int minHeightToDisplay = nbVisibleTiles * Constants.TILE_SIZE;

		float ratio = screenHeight / (float)minHeightToDisplay;

		pixelSize = (int)Mathf.Floor(ratio);
		if(pixelSize <= 0) {
			pixelSize = 1;
		}

		int newScreenHeight = screenHeight;

		int divider = 2 * pixelSize;

		int nbSparePixels = screenHeight % divider;
		if(nbSparePixels > 0) {
			newScreenHeight -= nbSparePixels;
		}

		int newScreenWidth = screenWidth;
		newScreenWidth -= newScreenWidth % 2;//remove extra pixel that can lead to glitches

		Camera cam = GetComponent<Camera>();

		//set the viewport rect to have a pixel perfect calculation of the orthographic size,
		// it must be a multiplier of the pixel size,
		// ex : if the pixel size is 5, if the cam height is 524 => new height will be 520, spare pixels will be 4
		//spare pixels will be on top of the hud (not a big deal)
		cam.pixelRect = new Rect(0, 0, newScreenWidth, newScreenHeight);

		//set the orthographic size to scale the scene with the pixelSize
		cam.orthographicSize = newScreenHeight / (float)divider;

		//Debug.Log(">>> " + Screen.width + " > " + newScreenWidth);
		//Debug.Log(">>> " + Screen.height + " > " + newScreenHeight + " > " + cam.orthographicSize + " > " + multiplier + " > " + nbSparePixels);
	}


}