using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class MessageDisplayer : MonoBehaviour {
	
	private static MessageDisplayer instance;
	
	private MessageDisplayer() {}
	
	public static MessageDisplayer Instance {
		get {
			return GameHelper.Instance.getMessageDisplayer();
		}
	}

	private List<Message> queue = new List<Message>();

	private Message currentMessage;

	private Coroutine coroutineShowBigMessage;


	
	public void displayMessage(string text) {
		displayMessages(new Message(text));
	}

	public void displayMessage(Message message) {
		displayMessages(message);
	}

	public void displayMessages(params Message[] messages) {
		displayMessages(false, messages);
	}

	public void displayMessages(bool isPrior, params Message[] messages) {

		if(messages == null) {
			return;
		}
		if(messages.Length <= 0) {
			return;
		}

		if(isPrior) {

			if(currentMessage != null) {
				queue.Insert(0, currentMessage);
			}
			
			queue.InsertRange(0, messages);

		} else {

			queue.AddRange(messages);
		}

		if(currentMessage == null) {
			displayNextMessage();
		}

	}

	public void displayNextMessage() {

		hideNormalMessage();

		if(queue.Count <= 0) {
			return;
		}

		Message message = queue[0];
		queue.RemoveAt(0);

		showNormalMessage(message);
	}

	public void displayBigMessage(string text, bool isPositive) {

		text = text.ToUpper();

		if(coroutineShowBigMessage != null) {
			StopCoroutine(coroutineShowBigMessage);
		}
		coroutineShowBigMessage = StartCoroutine(showBigMessage(text, isPositive));
	
	}

	private void showNormalMessage(Message message) {

		currentMessage = message;

		GameObject messageObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_NORMAL);
		GameObject messageActionObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_ACTION);
		GameObject backgroundObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BACKGROUND_MESSAGE_NORMAL);

		Text textComponent = messageObject.GetComponent<Text>();
		Text textActionComponent = messageActionObject.GetComponent<Text>();
		Image imageComponent = backgroundObject.GetComponent<Image>();

		string text = message.text;

		textComponent.enabled = true;
		textActionComponent.enabled = true;
		imageComponent.enabled = true;

		textComponent.text = text;
		textActionComponent.text = "Valider";
	}

	private void hideNormalMessage() {

		currentMessage = null;

		GameObject messageObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_NORMAL);
		GameObject messageActionObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_ACTION);
		GameObject backgroundObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BACKGROUND_MESSAGE_NORMAL);
		
		Text textComponent = messageObject.GetComponent<Text>();
		Text textActionComponent = messageActionObject.GetComponent<Text>();
		Image imageComponent = backgroundObject.GetComponent<Image>();

		textComponent.text = "";
		textActionComponent.text = "";
		
		textComponent.enabled = false;
		textActionComponent.enabled = false;
		imageComponent.enabled = false;
	}


	IEnumerator showBigMessage(string text, bool isPositive) {

		GameObject messageObject = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_MESSAGE_BIG);
		GameObject backgroundObject = GameObject.Find(Constants.GAME_OBJECT_NAME_BACKGROUND_MESSAGE_BIG);

		Text textComponent = messageObject.GetComponent<Text>();
		Image imageComponent = backgroundObject.GetComponent<Image>();

		textComponent.enabled = true;
		imageComponent.enabled = true;

		textComponent.text = text;
		if(isPositive) {
			textComponent.color = Color.green;
		} else {
			textComponent.color = Color.red;
		}

		yield return new WaitForSeconds(4f);

		textComponent.text = "";

		textComponent.enabled = false;	
		imageComponent.enabled = false;

	}

}

