using UnityEngine;
using Level;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
	
	public void init(bool activated) {
		
		this.isActivated = activated;
		
		if(activated) {
			GetComponent<SpriteRenderer>().sprite = spriteActivated;
		} else {
			GetComponent<SpriteRenderer>().sprite = spriteDeactivated;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {

			if(!isActivated) {
				proposeActivating();
			} else {
				proposeUsing();
			}

		}
		
	} 
	
	private void proposeActivating() {
		
		//TODO TEST
		activate();
	}

	private void proposeUsing() {
		
		//TODO TEST
		use();
	}

	private void activate() {
		
		//propose to activate
		setActivated(true);
		
		//keep level to respawn after
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		player.levelNameForlastHub = GameHelper.Instance.getLevelManager().getCurrentLevelName();
		
		GameSaver.Instance.getSaverHub().saveData();
		GameSaver.Instance.getSaverPlayerStats().saveData();

		MessageDisplayer.Instance.displayBigMessage("Hub activé");

	}

	private void use() {
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		player.reinitLifeAndStamina();
		
		//TODO respawn all enemies
		
		//TODO propose to manage experience / teleport

		
		//keep level to respawn after
		player.levelNameForlastHub = GameHelper.Instance.getLevelManager().getCurrentLevelName();
		
		GameSaver.Instance.getSaverPlayerStats().saveData();
		GameSaver.Instance.getSaverPlayerPosition().saveData();
	
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

	
	public virtual void serialize(BinaryFormatter bf, FileStream f) {
		bf.Serialize(f, isActivated);
	}
	
	public virtual void unserialize(BinaryFormatter bf, FileStream f) {
		isActivated = (bool) bf.Deserialize(f);
	}

}
