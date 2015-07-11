using System;
using UnityEngine;


public class HUD : MonoBehaviour {

	void FixedUpdate() {

		GameObject camera = GameHelper.Instance.getMainCamera();
		CameraResizer cameraResizer = camera.GetComponent<CameraResizer>();
		
		int pixelSize = cameraResizer.pixelSize;
		if(pixelSize <= 0) {
			return;
		}

		int nbVisibleTiles = cameraResizer.nbVisibleTiles;
		
		Transform background = transform.Find("HUDBackground");
		
		Vector2 position = camera.transform.position;
		Vector2 scale = transform.localScale;
		
		scale.x = (int)(Screen.width * Constants.TILE_SIZE / pixelSize) + Constants.TILE_SIZE;
		scale.y = Constants.TILE_SIZE;

		position.x -= (int)(Screen.width / pixelSize / 2f) + 1;
		position.y += (int)(Screen.height / pixelSize / 2f);

		transform.position = position;
		background.localScale = scale;

	}

}

