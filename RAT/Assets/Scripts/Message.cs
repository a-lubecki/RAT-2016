using System;


public class Message {

	public object caller { get; private set; }
	public string text { get; private set; }


	public Message (string text) : this(null, text) {

	}

	public Message (object caller, string text) {
	
		this.caller = caller;//can be null

		if(string.IsNullOrEmpty(text)) {
			this.text = "...";
		} else {
			this.text = text;
		}
	}

}

