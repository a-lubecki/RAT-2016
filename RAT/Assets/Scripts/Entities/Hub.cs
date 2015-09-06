using UnityEngine;
using Level;
using System.Collections;

public class Hub : MonoBehaviour, IActionnable {
	
	public bool isActivated { get; private set; }

	public NodeElementHub nodeElementHub { get; private set; }
	
	private Sprite spriteDeactivated;
	private Sprite spriteActivated;
	
	private CircleCollider2D getTriggerCollider() {
		return GetComponent<CircleCollider2D>();
	}

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

	void OnTriggerStay2D(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(collider.name)) {
			return;
		}
		
		if(!isActivated) {
			PlayerActionsManager.Instance.showAction(new ActionActivateHub(this));
		} else {
			PlayerActionsManager.Instance.showAction(new ActionUseHub(this));
		}
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(collider.name)) {
			return;
		}
		
		PlayerActionsManager.Instance.hideAction(new ActionActivateHub(this));
		PlayerActionsManager.Instance.hideAction(new ActionUseHub(this));

	}

	void IActionnable.notifyAction(BaseAction action) {

		if(!isActivated) {
			activate();
		} else {
			use();
		}
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

		MessageDisplayer.Instance.displayBigMessage("Hub activé", true);

		StartCoroutine(delayPlayerAfterAction());
	}
	
	private IEnumerator delayPlayerAfterAction() {
		
		PlayerControls playerControls = GameHelper.Instance.getPlayerControls();

		playerControls.disableControls();
		getTriggerCollider().enabled = false;

		yield return new WaitForSeconds(1f);
		
		getTriggerCollider().enabled = true;
		playerControls.enableControls();

	}

	private void use() {
		
		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		player.reinitLifeAndStamina();
		
		//respawn all enemies
		Npc[] npcs = GameHelper.Instance.getNpcs();
		foreach(Npc npc in npcs) {
			npc.reinitLifeAndPosition();
		}

		
		//keep level to respawn after
		player.levelNameForLastHub = GameHelper.Instance.getLevelManager().getCurrentLevelName();
		
		GameSaver.Instance.savePlayer();
		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.deleteNpcs();

		openHubMenu();

	}

	private void openHubMenu() {
		
		//disable controls when editing
		GameHelper.Instance.getPlayerControls().disableControls();//TODO disable move controls
		getTriggerCollider().enabled = false;

		//TODO propose to manage experience / teleport

		onPlayerStatsChanged();//TODO test call

		closeHubMenu();//TODO test call
	}
	
	private void closeHubMenu() {
		
		StartCoroutine(delayPlayerAfterAction());
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
