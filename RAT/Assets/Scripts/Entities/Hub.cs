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
		player.levelNameForLastHub = GameHelper.Instance.getLevelManager().getCurrentLevelName();
		
		GameSaver.Instance.saveHub();
		GameSaver.Instance.savePlayer();
		GameSaver.Instance.savePlayerStats();

		MessageDisplayer.Instance.displayBigMessage("Hub activé");

	}

	private void use() {
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		player.reinitLifeAndStamina();
		
		//respawn all enemies
		Npc[] npcs = GameHelper.Instance.getNpcs();
		foreach(Npc npc in npcs) {
			npc.reinitLifeAndPosition();
		}

		//TODO propose to manage experience / teleport

		
		//keep level to respawn after
		player.levelNameForLastHub = GameHelper.Instance.getLevelManager().getCurrentLevelName();

		GameSaver.Instance.savePlayer();
		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.saveNpcs();
	

		onPlayerStatsChanged();//TODO test call

	}

	public void onPlayerStatsChanged() {
		
		GameSaver.Instance.savePlayerStats();
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
