using UnityEngine;
using Level;

public class Hub : MonoBehaviour {
	
	public bool isActivated { get; private set; }

	public NodeElementHub nodeElementHub { get; private set; }
	
	private Sprite spriteDeactivated;
	private Sprite spriteActivated;
	
	public void setNodeElementHub(NodeElementHub nodeElementHub) {
		
		if(nodeElementHub == null) {
			throw new System.InvalidOperationException();
		}
		
		this.nodeElementHub = nodeElementHub;

		//load the images
		Texture2D textureDeactivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Deactivated.png");
		spriteDeactivated = Sprite.Create(textureDeactivated, 
		                                   new Rect(0, 0, textureDeactivated.width, textureDeactivated.height),
		                                   new Vector2(0.25f, 0.75f),
		                                   Constants.TILE_SIZE * 2);

		Texture2D textureActivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Activated.png");
		spriteActivated = Sprite.Create(textureActivated, 
		                                 new Rect(0, 0, textureDeactivated.width, textureDeactivated.height),
		                                 new Vector2(0.25f, 0.75f),
		                                 Constants.TILE_SIZE * 2);

	}
	
	void OnTriggerEnter2D(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_CONTROLS.Equals(other.name)) {

			if(!isActivated) {
				//propose to activate
				setActivated(true);//TODO test

				MessageDisplayer.Instance.displayBigMessage("Hub activé");

				//TODO delegate open doors in first level

			} else {
				//propose to manage experience / teleport
				//TODO
			}

		}
		
	} 


	public void setActivated(bool activated) {

		bool hasChanged = (this.isActivated != activated);
		
		this.isActivated = activated;

		if(activated) {

			GetComponent<SpriteRenderer>().sprite = spriteActivated;

			//notify lister
			if(hasChanged) {
				this.nodeElementHub.trigger(NodeElementHub.LISTENER_CALL_onHubActivated);
			}

		} else {

			GetComponent<SpriteRenderer>().sprite = spriteDeactivated;
			
			//notify lister
			if(hasChanged) {
				this.nodeElementHub.trigger(NodeElementHub.LISTENER_CALL_onHubDeactivated);
			}
		}

	}



}
