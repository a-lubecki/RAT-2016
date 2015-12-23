using System;

public abstract class Displayable {

	public readonly string trKey;

	public Displayable(string trKey) {

		if(string.IsNullOrEmpty(trKey)) {
			throw new System.ArgumentException();
		}

		this.trKey = trKey;
	}
	
	public string getTrName() {
		return Constants.tr(trKey);
	}

}

