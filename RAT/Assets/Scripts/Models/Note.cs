using System;
using System.Collections.Generic;
using UnityEngine;
using Node;

public class Note : BaseListenerModel {

	public static readonly int MAX_IMAGES_NOTES = 3;
	public static readonly string KEY_NAME_PREFIX_NOTE = "Note.";

	private static string[] getNoteTexts(NodeElementNote nodeElementNote) {
		
		int nbTexts = nodeElementNote.getTextCount();
		string[] texts = new string[nbTexts];

		for(int i = 0 ; i < nbTexts ; i++) {
			texts[i] = nodeElementNote.getText(i);
		}

		return texts;
	}


	private string[] trTexts;
	public string imageKeyName { get; private set; }

	public Note(NodeElementNote nodeElementNote) 
		: this(BaseListenerModel.getListeners(nodeElementNote),
			Note.getNoteTexts(nodeElementNote)) {

	}

	public Note(List<Listener> listeners, string[] trTexts) 
		: base(listeners) {

		this.trTexts = trTexts;

		//generate image keyname with a hash of trTexts to simulate a random
		int hash = 1;
		foreach(string text in trTexts) {
			hash = 31 * text.GetHashCode() + hash;
		}

		int num = hash % MAX_IMAGES_NOTES;
		if(num < 0) {
			num *= -1;
		}

		imageKeyName = KEY_NAME_PREFIX_NOTE + num;

	}


	public Message[] newMessages(object caller) {

		int nbTexts = trTexts.Length;
		Message[] res = new Message[nbTexts];

		for(int i = 0 ; i < nbTexts ; i++) {
			res[i] = new Message(caller, Constants.tr("Note." + trTexts[i]));
		}

		return res;
	}

}

