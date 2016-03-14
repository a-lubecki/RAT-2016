using System;
using System.Collections.Generic;

public abstract class BaseIdentifiableModel : BaseListenerModel {
		
	public string id { get; private set; }

	public BaseIdentifiableModel(string id, List<Listener> listeners) : base(listeners) {

		if(string.IsNullOrEmpty(id)) {
			throw new ArgumentException();
		}

		this.id = id;
	}

}

