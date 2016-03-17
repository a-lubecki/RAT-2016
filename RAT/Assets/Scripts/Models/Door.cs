using System;
using System.Collections.Generic;
using Node;

public class Door : BaseIdentifiableModel {

	public Orientation orientation { get ; private set; }
	public int spacing { get ; private set; }
	public bool hasUnlockSide { get ; private set; }
	public Direction unlockSide { get ; private set; }
	public ItemPattern requiredItemPattern { get ; private set; }

	public bool isOpened { get ; set; }


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

	}

}

