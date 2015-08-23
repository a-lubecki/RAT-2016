using System;


public class Message {

	public string text { get; private set; }

	public Message (string text) {

		if(string.IsNullOrEmpty(text)) {
			this.text = "...";
		} else {
			this.text = text;
		}
	}

}

