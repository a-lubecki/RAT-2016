using System;
using System.Collections.Generic;
using Node;
using MovementEffects;

public class Hub : BaseListenerModel, ISpawnable, IActionnable {

	public static readonly string LISTENER_CALL_onHubActivated = "onHubActivated";
	public static readonly string LISTENER_CALL_onHubDeactivated = "onHubDeactivated";

	public int posX { get; private set; }
	public int posY { get; private set; }
	public CharacterDirection spawnDirection { get; private set; }
	public bool isActivated { get; private set; }

	public bool hasTriggerActionCollider { get; private set; }


	public Hub(NodeElementHub nodeElementHub, bool isActivated) 
		: this(BaseListenerModel.getListeners(nodeElementHub), 
			nodeElementHub.nodePosition.x,
			nodeElementHub.nodePosition.y,
			isActivated) {
		
	}

	public Hub(List<Listener> listeners, int posX, int posY, bool isActivated) : base(listeners) {

		this.posX = posX;
		this.posY = posY;
		this.isActivated = isActivated;

		hasTriggerActionCollider = true;
	}

	int ISpawnable.getNextPosX() {
		return posX;
	}

	int ISpawnable.getNextPosY() {
		return posY;
	}

	CharacterDirection ISpawnable.getNextDirection() {
		return spawnDirection;
	}


	public void onEnterTriggerActionCollider() {

		if(!isActivated) {
			PlayerActionsManager.Instance.showAction(new ActionHubActivate(this));
		} else {
			PlayerActionsManager.Instance.showAction(new ActionHubUse(this));
		}
	}

	public void onExitTriggerActionCollider() {

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
		GameHelper.Instance.getPlayer().levelNameForLastHub = GameManager.Instance.getCurrentLevelName();

		GameManager.Instance.saveGame(false);

		MessageDisplayer.Instance.displayBigMessage(Constants.tr("BigMessage.HubActivated"), true);


		Timing.RunCoroutine(delayPlayerAfterAction());
	}

	private IEnumerator<float> delayPlayerAfterAction() {

		Player player = GameHelper.Instance.getPlayer();

		player.disableControls(this);

		hasTriggerActionCollider = false;
		updateBehaviors();

		yield return Timing.WaitForSeconds(1f);

		hasTriggerActionCollider = true;
		updateBehaviors();

		player.enableControls(this);

	}

	private void use() {

		Player player = GameHelper.Instance.getPlayer();
		player.reinitLifeAndStamina();

		//respawn all enemies
		Npc[] npcs = GameHelper.Instance.getNpcs();
		foreach(Npc npc in npcs) {

			if (npc.findGameObject<NpcBehavior>() == null) {
				
				npc.reinitLifeAndPosition();
			
			} else {
				
				//the game object was not previously created, create it using node
				NodeElementNpc nodeElementNpc = GameManager.Instance.getCurrentNodeLevel().findNpc(npc.id);

				new NpcCreator().createNewGameObject(nodeElementNpc, npc, false, 0, 0);
			}
		}


		//keep level to respawn after
		player.levelNameForLastHub = GameManager.Instance.getCurrentLevelName();

		GameManager.Instance.saveGame(true);

		openHubMenu();

	}

	private void openHubMenu() {

		Timing.RunCoroutine(delayPlayerAfterAction());

		GameHelper.Instance.getMenu().open(new MenuTypeHub(this));


		//TODO propose to manage experience / teleport

		onPlayerStatsChanged();//TODO test call

	}

	private void onPlayerStatsChanged() {

		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.saveAllToFile();
	}

	public void setActivated(bool activated) {

		bool hasChanged = (isActivated != activated);

		isActivated = activated;
		updateBehaviors();

		//notify lister
		if(hasChanged) {

			if(activated) {
				trigger(LISTENER_CALL_onHubActivated);
			} else {
				trigger(LISTENER_CALL_onHubDeactivated);
			}
		}

	}

}

