using System;

public abstract class Displayable {

	public readonly string trKey;

	public Displayable(string trKey) {

		if(string.IsNullOrEmpty(trKey)) {
			throw new System.ArgumentException();
		}

		this.trKey = trKey;
	}
	
	private string getName() {
		return Constants.tr(trKey);
	}

}

