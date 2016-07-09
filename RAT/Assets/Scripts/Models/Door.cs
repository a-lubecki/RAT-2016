using System;
using System.Collections;
using System.Collections.Generic;
using Node;
using MovementEffects;
using UnityEngine;

public class Door : BaseIdentifiableModel, IActionnable {

	public Orientation orientation { get ; private set; }
	public int spacing { get ; private set; }
	public bool hasUnlockSide { get ; private set; }
	public Direction unlockSide { get ; private set; }
	public ItemPattern requiredItemPattern { get ; private set; }

	public bool isOpened { get ; set; }

	private bool isAnimatingDoor = false;
	public float openingPercentage { get ; private set; }

	public bool hasTriggerActionCollider { get ; private set; }
	public bool hasTriggerMessageOutCollider { get ; private set; }


	public Door(NodeElementDoor nodeElementDoor, bool isOpened)
		: this(nodeElementDoor.nodeId.value,
			BaseListenerModel.getListeners(nodeElementDoor),
			nodeElementDoor.nodeOrientation.value,
			nodeElementDoor.nodeSpacing.value,
			nodeElementDoor.nodeUnlockSide != null,
			nodeElementDoor.nodeUnlockSide != null ? nodeElementDoor.nodeUnlockSide.value : Direction.NONE,
			nodeElementDoor.nodeRequireItem != null ? GameManager.Instance.getNodeGame().findItemPattern(nodeElementDoor.nodeRequireItem.value) : null,
			isOpened) {

		if(nodeElementDoor.nodeRequireItem != null && requiredItemPattern == null) {
			throw new InvalidOperationException("The item pattern was not found for the required item : " + nodeElementDoor.nodeRequireItem.value);
		}

	}

	public Door(string id, List<Listener> listeners, Orientation orientation, int spacing, bool hasUnlockSide, Direction unlockSide, ItemPattern requiredItem, bool isOpened) : base(id, listeners) {

		this.orientation = orientation;
		this.spacing = spacing;
		this.hasUnlockSide = hasUnlockSide;
		this.unlockSide = unlockSide;
		this.requiredItemPattern = requiredItem;
		this.isOpened = isOpened;

		isAnimatingDoor = false;
		openingPercentage = isOpened ? 1 : 0;

		hasTriggerActionCollider = !isOpened;
		hasTriggerMessageOutCollider = !isOpened;
	}


	public void setOpened(bool opened) {

		if (isOpened == opened) {
			return;
		}

		if (isAnimatingDoor) {
			return;
		}

		openingPercentage = opened ? 1 : 0;
		isOpened = opened;

		updateBehaviors();

	}

	public void open() {

		if (isOpened) {
			return;
		}

		animateDoor(true);

	}

	public void close() {

		if(!isOpened) {
			return;
		}

		animateDoor(false);
	}

	private void animateDoor(bool actionOpen) {

		if (isAnimatingDoor) {
			return;
		}

		isAnimatingDoor = true;

		hasTriggerActionCollider = false;
		hasTriggerMessageOutCollider = false;

		float timeFrame = 0.5f;
		float periodPercentage = Constants.COROUTINE_PERIOD_S / timeFrame;

		if (actionOpen) {
			//if open, hide the open action
			PlayerActionsManager.Instance.hideAction(new ActionDoorOpen(this));

		} else {
			//if close, decrement opening percentage
			periodPercentage *= -1;

			//close directly before animating
			isOpened = false;
		}


		updateBehaviors();


		Timing.CallPeriodically(timeFrame, Constants.COROUTINE_PERIOD_S, 
			delegate {

				//increment percentage
				openingPercentage += periodPercentage;

				if (openingPercentage < 0) {
					openingPercentage = 0;
				} else if (openingPercentage > 1) {
					openingPercentage = 1;
				}

				updateBehaviors();

			},
			delegate {

				isAnimatingDoor = false;

				setOpened(actionOpen);
			}
		);

	}


	public void onEnterTriggerActionCollider() {

		if(isOpened) {
			return;
		}

		PlayerActionsManager.Instance.showAction(new ActionDoorOpen(this));
	}

	public void onExitTriggerActionCollider() {

		if(isOpened) {
			return;
		}

		PlayerActionsManager.Instance.hideAction(new ActionDoorOpen(this));
	}

	public void onExitTriggerMessageOutCollider() {

		if(isOpened) {
			return;
		}

		//remove messages if player is exiting the larger zone
		MessageDisplayer.Instance.removeAllMessagesFrom(this);
	}


	void IActionnable.notifyActionShown(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionHidden(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		if(isOpened) {
			return;
		}

		Timing.RunCoroutine(delayPlayerAfterAction(), Segment.FixedUpdate);

	}


	public IEnumerator<float> delayPlayerAfterAction() {
		
		Player player = GameHelper.Instance.getPlayer();
		player.disableControls(this);

		hasTriggerActionCollider = false;
		hasTriggerMessageOutCollider = false;

		updateBehaviors();

		yield return Timing.WaitForSeconds(0.75f);

		manageDoorOpening();

		player.enableControls(this);

		if(!isOpened) {
			hasTriggerMessageOutCollider = true;
			updateBehaviors();
		}

		yield return Timing.WaitForSeconds(1f);

		//enable collider after delay to avoid displaying the action directly
		//with the message if the door is still closed  
		if(!isOpened) {
			hasTriggerActionCollider = true;
			updateBehaviors();
		}
	}


	private void manageDoorOpening() {

		//check if the player has to be in the right side to open the door
		if(hasUnlockSide) {

			if(unlockSide == Direction.NONE) {
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.Blocked")));
				return;
			}

			Player player = GameHelper.Instance.getPlayer();
			GameObject playerGameObject = player.findGameObject<PlayerBehavior>();
			if (playerGameObject == null) {
				throw new InvalidOperationException();
			}

			GameObject doorGameObject = findGameObject();
			if (doorGameObject == null) {
				throw new InvalidOperationException();
			}

			float x = doorGameObject.transform.position.x;
			float y = doorGameObject.transform.position.y;
			float xPlayer = playerGameObject.transform.position.x;
			float yPlayer = playerGameObject.transform.position.y;

			if((unlockSide == Direction.UP && y > yPlayer) ||
				(unlockSide == Direction.DOWN && y < yPlayer) ||
				(unlockSide == Direction.LEFT && x < xPlayer) ||
				(unlockSide == Direction.RIGHT && x > xPlayer)) {

				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.WrongSide")));
				return;
			}
		}

		if(requiredItemPattern != null) {

			if(!GameManager.Instance.getInventory().hasItemWithPattern(requiredItemPattern)) {
				//door remains locked
				MessageDisplayer.Instance.displayMessages(new Message(this, Constants.tr("Message.Door.Locked")));
				return;
			}

			//door unlocked with item
			MessageDisplayer.Instance.displayMessages(new Message(this, string.Format(Constants.tr("Message.Door.Unlock"), requiredItemPattern.getTrName())));

		}

		open();

	}

}

