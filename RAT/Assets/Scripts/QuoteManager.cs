using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuoteManager : MonoBehaviour {
	
	public static bool TEST = false;

	private static readonly Dictionary<string, string> authors = new Dictionary<string, string>() {
		{"GANDHI", "Gandhi"},
		{"AUGUSTE_COMPTE", "Auguste Compte"},
		{"PIERRE_CURIE", "Pierre Curie"},
		{"ALBERT_EINSTEIN", "Albert Einstein"},
		{"EDWARD_D_MORRISON", "Edward D. Morrison"}
	};

	private static readonly string[] quoteTrKeys = new string[] {
		"Quote.AUGUSTE_COMPTE.0",
		"Quote.GANDHI.0",
		"Quote.GANDHI.1",
		"Quote.GANDHI.2",
		"Quote.GANDHI.3",
		"Quote.GANDHI.4",
		"Quote.GANDHI.5",
		"Quote.PIERRE_CURIE.0"
	}; 

	
	public Text textQuote;
	public Text textAuthor;


	void Start () {
				
		if(TEST && Debug.isDebugBuild) {
			//test all
			StartCoroutine(test());

		} else {

			chooseQuote((int)(UnityEngine.Random.value * quoteTrKeys.Length));

			StartCoroutine(launchLevel());
		}

	}

	private void chooseQuote(int pos) {

		string quoteTrKey = quoteTrKeys[pos];

		textQuote.text = "\"" + Constants.tr(quoteTrKey) + "\"";
		
		string key = getAuthorKey(quoteTrKey);
		string authorName = null;

		foreach(KeyValuePair<string, string> entry in authors) {

			if(key.Equals(entry.Key)) {
				authorName = entry.Value;
				break;
			}
		}

		if(authorName != null) {
			//should never happen
			textAuthor.text = "- " + authorName + " -";
		}

	}

	private string getAuthorKey(string quote) {
		return quote.Split(new string[] {"."}, System.StringSplitOptions.None)[1];
	}

	private IEnumerator launchLevel() {

		yield return new WaitForSeconds(6);

		//load the current level
		if(!GameSaver.Instance.loadCurrentLevel()) {
			
			//if no saved data, load the very first level				
			GameHelper.Instance.getLevelManager().loadNextLevel(Constants.FIRST_LEVEL_NAME);

		}
	}

	private IEnumerator test() {

		for(int i=0 ; i<quoteTrKeys.Length ; i++) {

			chooseQuote(i);

			yield return new WaitForSeconds(3);
		}
		
	}

}

