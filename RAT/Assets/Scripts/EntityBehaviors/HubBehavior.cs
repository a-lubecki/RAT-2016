using UnityEngine;
using Node;
using System;
using System.Collections;

public class HubBehavior : MonoBehaviour, IActionnable {

	private static Sprite spriteDeactivated;
	private static Sprite spriteActivated;

	public Hub hub { get; private set; }


	void Awake() {

		//load the images
		if(spriteDeactivated == null) {
			Texture2D textureDeactivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Deactivated");
			spriteDeactivated = Sprite.Create(textureDeactivated, 
				new Rect(0, 0, textureDeactivated.width, textureDeactivated.height),
				new Vector2(0.25f, 0.75f),
				Constants.TILE_SIZE * 2);
		}

		if(spriteActivated == null) {
			Texture2D textureActivated = GameHelper.Instance.loadTexture2DAsset(Constants.PATH_RES_ENVIRONMENTS + "Hub.Activated");
			spriteActivated = Sprite.Create(textureActivated, 
				new Rect(0, 0, textureActivated.width, textureActivated.height),
				new Vector2(0.25f, 0.75f),
				Constants.TILE_SIZE * 2);
		}
	}

	public void init(Hub hub) {

		if(hub == null) {
			throw new ArgumentException();
		}

		this.hub = hub;
		
		if(hub.isActivated) {
			GetComponent<SpriteRenderer>().sprite = spriteActivated;
		} else {
			GetComponent<SpriteRenderer>().sprite = spriteDeactivated;
		}
	}

	private CircleCollider2D getTriggerCollider() {
		return GetComponent<CircleCollider2D>();
	}

	void OnTriggerStay2D(Collider2D collider) {
		
		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}
		
		if(!hub.isActivated) {
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


	void IActionnable.notifyActionShown(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionHidden(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		if(!hub.isActivated) {
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
		player.levelNameForLastHub = GameManager.Instance.getCurrentLevelName();

		GameManager.Instance.saveGame(false);

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
		player.levelNameForLastHub = GameManager.Instance.getCurrentLevelName();

		GameManager.Instance.saveGame(true);

		openHubMenu();

	}

	private void openHubMenu() {
		
		//disable controls when editing
		GameHelper.Instance.getPlayer().disableControls();
		getTriggerCollider().enabled = false;

		GameHelper.Instance.getMenu().open(new MenuTypeHub(hub));
		
		StartCoroutine(delayPlayerAfterAction());


		//TODO propose to manage experience / teleport

		onPlayerStatsChanged();//TODO test call

	}


	public void onPlayerStatsChanged() {
		
		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.saveAllToFile();
	}

	public void setActivated(bool activated) {

		bool hasChanged = (hub.isActivated != activated);
		
		hub.setActivated(activated);

		if(activated) {

			GetComponent<SpriteRenderer>().sprite = spriteActivated;

			//notify lister
			if(hasChanged) {
				hub.trigger(Hub.LISTENER_CALL_onHubActivated);
			}

		} else {

			GetComponent<SpriteRenderer>().sprite = spriteDeactivated;
			
			//notify lister
			if(hasChanged) {
				hub.trigger(Hub.LISTENER_CALL_onHubDeactivated);
			}
		}

	}

}
