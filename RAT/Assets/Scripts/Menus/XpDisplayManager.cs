using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XpDisplayManager : MonoBehaviour {

	private int earnedXp;

	private Coroutine couroutineDisplayEarnedXp;

	private void Awake() {
		setTotalXp(0);
		setEarnedXp(0);
	}

	public void setTotalXp(int xp) {
		
		GameObject objectXpTotal = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_XP_TOTAL);
		Text textXpTotal = objectXpTotal.GetComponent<Text>();
		
		textXpTotal.text = "" + xp;
	}

	private void setEarnedXp(int xp) {
		
		GameObject objectXpEarned = GameObject.Find(Constants.GAME_OBJECT_NAME_TEXT_XP_EARNED);
		Text textXpEarned = objectXpEarned.GetComponent<Text>();

		if(xp == 0) {
			textXpEarned.text = "";
		} else if(xp > 0) {
			textXpEarned.text = "+" + xp;
		} else {
			textXpEarned.text = "" + xp;
		}
	}

	public void earnXp(int lastXp, int xp) {

		if(xp <= 0) {
			return;
		}

		setTotalXp(lastXp + xp);
		
		earnedXp += xp;

		if(couroutineDisplayEarnedXp != null) {
			//already earned xp
			StopCoroutine(couroutineDisplayEarnedXp);
		}

		couroutineDisplayEarnedXp = StartCoroutine(displayEarnedXp());

	}


	private IEnumerator displayEarnedXp() {

		setEarnedXp(earnedXp);

		yield return new WaitForSeconds(2f);

		setEarnedXp(0);
		earnedXp = 0;

		couroutineDisplayEarnedXp = null;
	}

}


