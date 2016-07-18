using System;
using System.Collections.Generic;
using UnityEngine;
using Node;
using MovementEffects;

public class Note : BaseListenerModel, IActionnable {

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

	private bool isColliding = false;

	public bool hasTriggerActionCollider { get ; private set; }
	public bool hasTriggerMessageOutCollider { get ; private set; }


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

		hasTriggerActionCollider = true;
		hasTriggerMessageOutCollider = true;
	}


	public Message[] newMessages(object caller) {

		int nbTexts = trTexts.Length;
		Message[] res = new Message[nbTexts];

		for(int i = 0 ; i < nbTexts ; i++) {
			res[i] = new Message(caller, Constants.tr("Note." + trTexts[i]));
		}

		return res;
	}


	public void onEnterTriggerActionCollider() {

		if(isColliding) {
			return;
		}

		if(!MessageDisplayer.Instance.isShowingMessageFrom(this)) {

			PlayerActionsManager.Instance.showAction(new ActionNoteShow(this));
			isColliding = PlayerActionsManager.Instance.isShowingAction(this);
		}

	}

	public void onExitTriggerActionCollider() {

		if(!isColliding) {
			return;
		}

		isColliding = false;
		PlayerActionsManager.Instance.hideAction(new ActionNoteShow(this));

	}

	public void onExitTriggerMessageCollider() {

		//remove messages if player is exiting the larger zone
		MessageDisplayer.Instance.removeAllMessagesFrom(this);
	}


	void IActionnable.notifyActionShown(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionHidden(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		Timing.RunCoroutine(delayPlayerAfterAction(), Segment.FixedUpdate);

	}

	private IEnumerator<float> delayPlayerAfterAction() {

		Player player = GameHelper.Instance.getPlayer();

		player.disableControls(this);

		hasTriggerActionCollider = false;
		hasTriggerMessageOutCollider = false;
		updateBehaviors();

		yield return Timing.WaitForSeconds(0.75f);

		MessageDisplayer.Instance.displayMessages(newMessages(this));

		player.enableControls(this);

		hasTriggerMessageOutCollider = true;
		updateBehaviors();

		yield return Timing.WaitForSeconds(1f);

		//enable collider after delay to avoid displaying the action directly
		//with the message if the door is still closed
		hasTriggerActionCollider = true;
		updateBehaviors();

	}


}

