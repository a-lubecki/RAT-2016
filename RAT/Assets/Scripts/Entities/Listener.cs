using System;

public class Listener {

	public string inputCallName { get; private set; }
	public string outputCallName { get; private set; }

	public Listener(string input, string output) {

		if(string.IsNullOrEmpty(input)) {
			throw new ArgumentException();
		}
		if(string.IsNullOrEmpty(output)) {
			throw new ArgumentException();
		}

		this.inputCallName = input;
		this.outputCallName = output;

	}


}

