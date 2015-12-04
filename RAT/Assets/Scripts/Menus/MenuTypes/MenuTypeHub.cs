using System;

public class MenuTypeHub : AbstractMenuType {

	private Hub hub;

	public MenuTypeHub(Hub hub) : base() {
		
		if(hub == null) {
			throw new System.ArgumentException();
		}

		this.hub = hub;

	}
	
}

