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
		Texture2D textureDeactivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Deactivated");
		spriteDeactivated = Sprite.Create(textureDeactivated, 
		                                   new Rect(0, 0, textureDeactivated.width, textureDeactivated.height),
		                                   new Vector2(0.25f, 0.75f),
		                                   Constants.TILE_SIZE * 2);

		Texture2D textureActivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Activated");
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
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}
		
		if(!isActivated) {
			PlayerActionsManager.Instance.showAction(new ActionHubActivate(this));
		} else {
			PlayerActionsManager.Instance.showAction(new ActionHubUse(this));
		}
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}
		
		PlayerActionsManager.Instance.hideAction(new ActionHubActivate(this));
		PlayerActionsManager.Instance.hideAction(new ActionHubUse(this));
		
		GameHelper.Instance.getMenu().close(typeof(MenuTypeHub));
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
		Player player = GameHelper.Instance.getPlayer();
		player.levelNameForLastHub = GameHelper.Instance.getLevelManager().getCurrentLevelName();
		
		GameSaver.Instance.saveHub();
		GameSaver.Instance.savePlayer();
		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.saveAllToFile();

		MessageDisplayer.Instance.displayBigMessage(Constants.tr("BigMessage.HubActivated"), true);

		StartCoroutine(delayPlayerAfterAction());
	}
	
	private IEnumerator delayPlayerAfterAction() {
		
		Player player = GameHelper.Instance.getPlayer();

		player.disableControls();
		getTriggerCollider().enabled = false;

		yield return new WaitForSeconds(1f);
		
		getTriggerCollider().enabled = true;
		player.enableControls();

	}

	private void use() {
		
		Player player = GameHelper.Instance.getPlayer();
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
		GameSaver.Instance.saveAllToFile();

		openHubMenu();

	}

	private void openHubMenu() {
		
		//disable controls when editing
		GameHelper.Instance.getPlayer().disableControls();
		getTriggerCollider().enabled = false;

		GameHelper.Instance.getMenu().open(new MenuTypeHub(this));
		
		StartCoroutine(delayPlayerAfterAction());


		//TODO propose to manage experience / teleport

		onPlayerStatsChanged();//TODO test call

	}


	public void onPlayerStatsChanged() {
		
		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.saveAllToFile();
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
