using System;
using System.Collections.Generic;
using Node;

public class Hub : BaseListenerModel {

	public static readonly string LISTENER_CALL_onHubActivated = "onHubActivated";
	public static readonly string LISTENER_CALL_onHubDeactivated = "onHubDeactivated";

	public Direction spawnDirection { get; private set; }
	public bool isActivated { get; private set; }

	public Hub(NodeElementHub nodeElementHub, bool isActivated) : this(getListeners(nodeElementHub), isActivated) {
		
	}

	public Hub(List<Listener> listeners, bool isActivated) : base(listeners) {

		this.isActivated = isActivated;

	}

	public void setActivated(bool isActivated) {
		this.isActivated = isActivated;
	}

}

