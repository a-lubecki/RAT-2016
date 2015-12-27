using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuoteManager : MonoBehaviour {
	
	private static readonly string AUTHOR_GANDHI = "Gandhi";
	private static readonly string AUTHOR_AUGUSTE_COMPTE = "Auguste Compte";
	private static readonly string AUTHOR_PIERRE_CURIE = "Pierre Curie";

	private static readonly string AUTHOR_ALBERT_EINSTEIN = "Albert Einstein";
	private static readonly string AUTHOR_EDWARD_D_MORRISON = "Edward D. Morrison";

	public Text textQuote;
	public Text textAuthor;


	private static readonly Quote[] quotes = new Quote[] {
		new Quote("Quote.AUGUSTE.COMPTE.0", AUTHOR_AUGUSTE_COMPTE),
		new Quote("Quote.GANDHI.0", AUTHOR_GANDHI),
		new Quote("Quote.GANDHI.1", AUTHOR_GANDHI),
		new Quote("Quote.GANDHI.2", AUTHOR_GANDHI),
		new Quote("Quote.GANDHI.3", AUTHOR_GANDHI),
		new Quote("Quote.GANDHI.4", AUTHOR_GANDHI),
		new Quote("Quote.GANDHI.5", AUTHOR_GANDHI),
		new Quote("Quote.PIERRE.CURIE.0", AUTHOR_PIERRE_CURIE)
	}; 

	
	private class Quote {
		
		public readonly string quoteTrKey;
		public readonly string author;
		
		public Quote(string quoteKey, string author) {
			this.quoteTrKey = quoteKey;
			this.author = author;
		}
	}


	void Start () {

		Quote quote = quotes[(int)(UnityEngine.Random.value * quotes.Length)];

		textQuote.text = "\"" + Constants.tr(quote.quoteTrKey) + "\"";
		textAuthor.text = "- " + quote.author + " -";

		StartCoroutine(launchLevel());
	}

	private IEnumerator launchLevel() {

		yield return new WaitForSeconds(6);

		//load the current level
		if(!GameSaver.Instance.loadCurrentLevel()) {
			
			//if no saved data, load the very first level				
			GameHelper.Instance.getLevelManager().loadNextLevel(Constants.FIRST_LEVEL_NAME);

		}
	}

}

