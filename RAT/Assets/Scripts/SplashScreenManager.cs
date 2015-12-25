using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
	
public class SplashScreenManager : MonoBehaviour {

	
	public static readonly string GAME_TITLE = "R.A.T.";
	public static readonly string GAME_SUBTITLE = "Rush\nAnd\nTear";

	public static readonly string GAME_CREDITS = GAME_TITLE + " - 2015/2016 - Aurélien Lubecki - v" + Application.version;


	void Start() {

		StartCoroutine(startAnimation());

	}

	IEnumerator startAnimation() {
		
		Image background = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_BACKGROUND).GetComponent<Image>();
		Image foreground = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_FOREGROUND).GetComponent<Image>();
		
		Image splatTitle = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_TITLE).GetComponent<Image>();
		Image splatCredits = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SPLAT_CREDITS).GetComponent<Image>();

		Text title = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_TITLE).GetComponent<Text>();
		Text subTitle = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_SUBTITLE).GetComponent<Text>();
		Text credits = GameObject.Find(Constants.GAME_OBJECT_NAME_SPLASHSCREEN_CREDITS).GetComponent<Text>();
		
		title.text = GAME_TITLE;
		subTitle.text = GAME_SUBTITLE;
		credits.text = GAME_CREDITS;

		setAlpha(foreground, 0);

		setAlpha(splatTitle, 0);
		setAlpha(splatCredits, 0);

		setAlpha(title, 0);
		setAlpha(subTitle, 0);
		setAlpha(credits, 0);

		for(int i = 0 ; i <= 10 ; i++) {
			
			setAlpha(background, (float)i / 10f);
			
			yield return new WaitForSeconds(0.05f);
		}
		
		yield return new WaitForSeconds(1);

		background.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_SPLASHSCREEN + "SplashScreenBgAfter.png");
		
		StartCoroutine(hideImage(foreground));

		StartCoroutine(shakeImage(background));


		yield return new WaitForSeconds(2);

		setAlpha(splatTitle, 1);
		
		yield return new WaitForSeconds(0.2f);

		setAlpha(title, 1);

		yield return new WaitForSeconds(0.2f);

		setAlpha(subTitle, 1);


		yield return new WaitForSeconds(1);

		setAlpha(splatCredits, 1);
		
		yield return new WaitForSeconds(0.2f);

		setAlpha(credits, 1);

	}

	/*
	private IEnumerator revealText(Text text) {
		
		for(int i = 0 ; i <= 10 ; i++) {

			setAlpha(text, (float)i / 10f);
			
			yield return new WaitForSeconds(0.05f);
		}

	}
*/
	
	private IEnumerator hideImage(Image image) {
		
		for(int i = 0 ; i <= 20 ; i++) {
			
			setAlpha(image, 1 - (float)i / 20f);
			
			yield return new WaitForSeconds(0.05f);
		}

		image.color = new Color(0, 0, 0, 0);

		//stroboscopic
		while(true) {

			setAlpha(image, UnityEngine.Random.value * 0.06f);

			yield return new WaitForSeconds(0.1f);

			setAlpha(image, 0);

			yield return new WaitForSeconds(UnityEngine.Random.value * 0.1f + 0.1f);

		}

	}
	
	private IEnumerator shakeImage(Image image) {

		Transform imageTransform = image.transform;

		yield return new WaitForSeconds(4);


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
				if(amplitude >= 0.1f) {
					amplitude -= 0.04f;
				}
			}

		}

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

}


