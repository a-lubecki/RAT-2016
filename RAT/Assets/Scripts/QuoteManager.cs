using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuoteManager : MonoBehaviour {
	
	public static bool TEST = false;

	private static readonly Dictionary<string, string> authors = new Dictionary<string, string>() {
		{"ALBERT_EINSTEIN", "Albert Einstein"},
		{"ALDOUS_HUXLEY", "Aldous Huxley"},
		{"AUGUSTE_COMPTE", "Auguste Compte"},
		{"CLAUDE_JOSEPH_ROUGET_DE_LISLE", "Claude Joseph Rouget de Lisle"},
		{"DAVID_HOMEL", "David Homel"},
		{"EDGAR_MORIN", "Edgar Morin"},
		{"EDWARD_D_MORRISON", "Edward D. Morrison"},
		{"FRIEDRICH_NIETZSCHE", "Friedrich Nietzsche"},
		{"GANDHI", "Gandhi"},
		{"HENRY_DE_MONTHERLANT", "Henry de Montherlant"},
		{"MICHAEL_CRICHTON", "Michael Crichton"},
		{"NAPOLEON_BONAPARTE", "Napoléon Bonaparte"},
		{"PIERRE_CURIE", "Pierre Curie"},
		{"RENE_DUBOS", "René Dubos"},
		{"TIM_WHITE", "Tim White"},
		{"WOODY_ALLEN", "Woody Allen"}
	};

	private static readonly string[] quoteTrKeys = new string[] {
		"Quote.ALBERT_EINSTEIN.0",
		"Quote.ALBERT_EINSTEIN.1",
		"Quote.ALBERT_EINSTEIN.2",
		"Quote.ALBERT_EINSTEIN.3",
		"Quote.ALDOUS_HUXLEY.0",
		"Quote.ANONYMOUS.0",
		"Quote.AUGUSTE_COMPTE.0",
		"Quote.CLAUDE_JOSEPH_ROUGET_DE_LISLE.0",
		"Quote.DAVID_HOMEL.0",
		"Quote.EDGAR_MORIN.0",
		"Quote.EDGAR_MORIN.1",
		"Quote.EDWARD_D_MORRISON.0",
		"Quote.FRIEDRICH_NIETZSCHE.0",
		"Quote.GANDHI.0",
		"Quote.GANDHI.1",
		"Quote.GANDHI.2",
		"Quote.GANDHI.3",
		"Quote.GANDHI.4",
		"Quote.GANDHI.5",
		"Quote.HENRY_DE_MONTHERLANT.0",
		"Quote.HENRY_DE_MONTHERLANT.1",
		"Quote.HENRY_DE_MONTHERLANT.2",
		"Quote.HENRY_DE_MONTHERLANT.3",
		"Quote.MICHAEL_CRICHTON.0",
		"Quote.NAPOLEON_BONAPARTE.0",
		"Quote.PIERRE_CURIE.0",
		"Quote.RENE_DUBOS.0",
		"Quote.TIM_WHITE.0",
		"Quote.WOODY_ALLEN.0"
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

		if(key.Equals("ANONYMOUS")) {

			authorName = Constants.tr("Author.ANONYMOUS");

		} else {

			foreach(KeyValuePair<string, string> entry in authors) {

				if(key.Equals(entry.Key)) {
					authorName = entry.Value;
					break;
				}
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

		yield return new WaitForSeconds(5);

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

