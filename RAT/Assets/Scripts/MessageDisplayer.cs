using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MessageDisplayer {
	
	private static MessageDisplayer instance;
	
	private MessageDisplayer() {}
	
	public static MessageDisplayer Instance {
		
		get {
			if (instance == null) {
				instance = new MessageDisplayer();
			}
			return instance;
		}
	}

	//private List<Message> queue = new List();


	public void displayMessage(string text) {

		int length = text.Length;

		displayMessage(text, Message.MAX_DURATION_SEC + length / 20f, 0f, false);
	}

	public void displayMessage(string text, float duration, float delay, bool isPrior) {

		Message message = new Message(text, duration, delay, isPrior);

		//TODO enqueue
		Debug.Log("displayMessage : " + message.text);

	}

	public void displayBigMessage(string text) {

		text = text.ToUpper();
		
		Debug.Log("displayBigMessage : " + text);
	}

}

