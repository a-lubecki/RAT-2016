using System;


public class Message {

	public static readonly int MAX_DURATION_SEC = 2;

	public string text { get; private set; }
	public float duration { get; private set; }
	public float delay { get; private set; }
	public bool isPrior { get; private set; }

	public Message (string text, float duration, float delay, bool isPrior) {

		if(string.IsNullOrEmpty(text)) {
			this.text = "...";
		} else {
			this.text = text;
		}

		if(duration < MAX_DURATION_SEC) {
			this.duration = MAX_DURATION_SEC;
		} else {
			this.duration = duration;
		}

		if(delay < 0) {
			this.delay = 0;
		} else {
			this.delay = delay;
		}
	
		this.isPrior = isPrior;
	}

}

