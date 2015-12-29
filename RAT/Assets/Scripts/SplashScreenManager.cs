using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
	
public class SplashScreenManager : MonoBehaviour {

	public static readonly string GAME_TITLE = "R.A.T.";
	public static readonly string GAME_SUBTITLE = "Rush\nAttack\nTry again";

	public static string getGameCreditsTitle() {
		return GAME_TITLE + " - " + GAME_SUBTITLE.Replace("\n", " ") + " - 2015/2016 - Aurélien Lubecki - v" + Application.version;
	}
	
	private bool inputsEnabled = false;
	private bool hasHiddenTitle = false;
	
	public GameObject buttonContinueGame;
	public GameObject buttonNewGame;
	public GameObject buttonCredits;
	
	public AudioSource backgroundMusicPlayer;
	public AudioSource audioSourceExplosion;

	protected void Start() {

		buttonContinueGame.SetActive(false);
		buttonNewGame.SetActive(false);
		buttonCredits.SetActive(false);

		StartCoroutine(startAnimation());
				
		StartCoroutine(showMainTitle());

	}
	
	public void hideSplashScreenTitle() {
		
		if(!inputsEnabled) {
			return;
		}
		
		if(hasHiddenTitle) {
			return;
		}

		StartCoroutine(showMenuButtons());
	}

	private static void setAlpha(Image image, float alpha) {
		
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}
	
	private static void setAlpha(Text text, float alpha) {
		
		Color color = text.color;
		color.a = alpha;
		text.color = color;
	}


	IEnumerator startAnimation() {
		
		Image background = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_BACKGROUND).GetComponent<Image>();
		Image foreground = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_FOREGROUND).GetComponent<Image>();
		
		setAlpha(background, 0);
		setAlpha(foreground, 0);
		
		yield return new WaitForSeconds(1.5f);


		for(int i = 0 ; i <= 15 ; i++) {
			
			setAlpha(background, (float)i / 15f);
			
			yield return new WaitForSeconds(0.05f);
		}


		yield return new WaitForSeconds(5.5f);


		background.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_SPLASHSCREEN + "SplashScreenBgAfter");

		
		for(int i = 0 ; i <= 5 ; i++) {
			
			setAlpha(foreground, (float)i / 5f);
			
			yield return new WaitForSeconds(0.05f);
		}


		yield return new WaitForSeconds(1.2f);


		StartCoroutine(hideImage(foreground));

		StartCoroutine(triggerStroboscopic(background));
		
		yield return new WaitForSeconds(1.2f);

		StartCoroutine(shakeImage(background));

		audioSourceExplosion.PlayDelayed(1.6f);

	}
	
	private IEnumerator hideImage(Image image) {
		
		for(int i = 0 ; i <= 150 ; i++) {
			
			setAlpha(image, 1 - (float)i / 150f);
			
			yield return new WaitForSeconds(0.05f);
		}

	}

	private IEnumerator triggerStroboscopic(Image image) {

		//stroboscopic
		while(true) {

			setAlpha(image, 1 - UnityEngine.Random.value * 0.2f);

			yield return new WaitForSeconds(0.1f);

			setAlpha(image, 1);

			yield return new WaitForSeconds(UnityEngine.Random.value * 0.1f + 0.1f);

		}

	}
	
	private IEnumerator shakeImage(Image image) {

		Transform imageTransform = image.transform;

		yield return new WaitForSeconds(2.5f);


		float amplitude = 0;
		float maxAmplitude = 4;

		bool ascending = true;

		while(amplitude >= 0) {
			
			float x = UnityEngine.Random.value * amplitude - amplitude/2f;
			float y = (UnityEngine.Random.value * amplitude - amplitude/2f) / 4f;

			imageTransform.localPosition = new Vector2(x, y);
			
			float delay = UnityEngine.Random.value * 0.01f + 0.04f;
			yield return new WaitForSeconds(delay);

			if(ascending) {
				amplitude += 0.02f;
				if(amplitude >= maxAmplitude) {
					ascending = false;
				}
			} else {
				if(amplitude >= 0.3f) {
					amplitude -= 0.04f;
				}
			}

		}

	}
	
	private IEnumerator showMainTitle() {
		
		Image splatTitle = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_TITLE).GetComponent<Image>();
		Image splatCredits = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_CREDITS).GetComponent<Image>();
		
		Text title = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_TITLE).GetComponent<Text>();
		Text subTitle = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SUBTITLE).GetComponent<Text>();
		Text credits = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_CREDITS).GetComponent<Text>();
		
		title.text = GAME_TITLE;
		subTitle.text = GAME_SUBTITLE;
		credits.text = getGameCreditsTitle();

		setAlpha(splatTitle, 0);
		setAlpha(splatCredits, 0);
		
		setAlpha(title, 0);
		setAlpha(subTitle, 0);
		setAlpha(credits, 0);

		yield return new WaitForSeconds(2.5f);
		
		for(int i = 0 ; i <= 3 ; i++) {
			
			float value = (float)i / 3f;
			setAlpha(splatCredits, value);
			setAlpha(credits, value);
			
			yield return new WaitForSeconds(0.05f);
		}


		backgroundMusicPlayer.PlayDelayed(0.5f);

		yield return new WaitForSeconds(1f);


		for(int i = 0 ; i <= 3 ; i++) {

			float value = (float)i / 3f;
			setAlpha(splatTitle, value);
			setAlpha(title, value);
			setAlpha(subTitle, value);
			
			yield return new WaitForSeconds(0.05f);
		}


		inputsEnabled = true;
	}


	private IEnumerator showMenuButtons() {

		if(hasHiddenTitle) {
			return false;
		}

		inputsEnabled = false;
		hasHiddenTitle = true;

		Image splatTitle = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_TITLE).GetComponent<Image>();

		Text title = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_TITLE).GetComponent<Text>();
		Text subTitle = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SUBTITLE).GetComponent<Text>();

		for(int i = 0 ; i <= 3 ; i++) {
			
			float value = 1 - (float)i / 3f;

			setAlpha(splatTitle, value);
			setAlpha(title, value);
			setAlpha(subTitle, value);
			
			yield return new WaitForSeconds(0.05f);
		}
		
		yield return new WaitForSeconds(0.5f);


		buttonContinueGame.SetActive(true);
		buttonNewGame.SetActive(true);
		buttonCredits.SetActive(true);

		inputsEnabled = true;

	}
	
	public void onButtonContinueGameClicked() {

		if(AutoFade.Fading) {
			return;
		}

		AutoFade.LoadLevel((int)(Constants.SceneIndex.SCENE_INDEX_QUOTE), 0.3f, 0.3f, Color.black);

	}

	public void onButtonNewGameClicked() {
		
		if(AutoFade.Fading) {
			return;
		}
		
		AutoFade.LoadLevel((int)(Constants.SceneIndex.SCENE_INDEX_QUOTE), 0.3f, 0.3f, Color.black);

	}

	public void onButtonCreditsClicked() {
		
	}

}


