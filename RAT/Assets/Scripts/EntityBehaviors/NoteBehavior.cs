using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Node;

public class NoteBehavior : MonoBehaviour, IActionnable {

	public Note note { get; private set; }

	private bool isColliding = false;

	public void init(Note note) {

		if(note == null) {
			throw new ArgumentException();
		}

		this.note = note;

	}

	private CircleCollider2D getTriggerActionInCollider() {
		return GetComponents<CircleCollider2D>()[0];
	}

	private CircleCollider2D getTriggerActionOutCollider() {
		return GetComponents<CircleCollider2D>()[1];
	}

	private CircleCollider2D getTriggerMessagesOutCollider() {
		return GetComponents<CircleCollider2D>()[2];
	}


	void OnTriggerStay2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(!isColliding) {

			if(getTriggerActionInCollider().IsTouching(collider)) {

				if(!MessageDisplayer.Instance.isShowingMessageFrom(this)) {
					
					isColliding = true;
					PlayerActionsManager.Instance.showAction(new ActionNoteShow(this));

				}

			}
		}

	}


	void OnTriggerExit2D(Collider2D collider) {

		if(!Constants.GAME_OBJECT_NAME_PLAYER.Equals(collider.name)) {
			return;
		}

		if(isColliding) {
			
			if(!getTriggerActionOutCollider().IsTouching(collider)) {

				isColliding = false;
				PlayerActionsManager.Instance.hideAction(new ActionNoteShow(this));

			}
		}

		if(!getTriggerMessagesOutCollider().IsTouching(collider)) {
			//remove messages if player is exiting the larger zone
			MessageDisplayer.Instance.removeAllMessagesFrom(this);
		}
	}


	void IActionnable.notifyActionShown(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionHidden(BaseAction action) {
		//do nothing
	}

	void IActionnable.notifyActionValidated(BaseAction action) {

		StartCoroutine(delayPlayerAfterAction());

	}


	private IEnumerator delayPlayerAfterAction() {

		PlayerBehavior playerBehavior = GameHelper.Instance.findPlayerBehavior();

		playerBehavior.disableControls();
		getTriggerActionInCollider().enabled = false;
		getTriggerMessagesOutCollider().enabled = false;


		yield return new WaitForSeconds(0.75f);

		showMessages();

		playerBehavior.enableControls();

		getTriggerMessagesOutCollider().enabled = true;

		yield return new WaitForSeconds(1f);

		//enable collider after delay to avoid displaying the action directly
		//with the message if the door is still closed  
		getTriggerActionInCollider().enabled = true;

	}

	private void showMessages() {
		
		MessageDisplayer.Instance.displayMessages(note.newMessages(this));

	}

}

