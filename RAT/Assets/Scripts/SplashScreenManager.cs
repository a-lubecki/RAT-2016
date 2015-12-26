using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
	
public class SplashScreenManager : MonoBehaviour {
	
	private readonly KeyCode[] KEYS_START = new KeyCode[] {
		KeyCode.Return,
		KeyCode.KeypadEnter
	};
	private readonly string[] BUTTONS_START = new string[] {
		"Action1",
		"Action2",
		"Start", 
		"Select",
		"TouchPadTap"//PS4
	};
	
	public static readonly string GAME_TITLE = "R.A.T.";
	public static readonly string GAME_SUBTITLE = "Rush\nAnd\nTear";

	public static readonly string GAME_CREDITS = GAME_TITLE + " - 2015/2016 - Aurélien Lubecki - v" + Application.version;
	
	private bool inputsEnabled = false;
	private bool hasHiddenTitle = false;
	
	public GameObject buttonContinueGame;
	public GameObject buttonNewGame;
	public GameObject buttonCredits;

	public AudioSource audioSourceExplosion;

	private static readonly int MAX_BUTTON_LONG_PRESS_ITERATIONS = 30;	//TODO move
	private Dictionary<string, int> buttonPressIterations = new Dictionary<string, int>();	//TODO move

	protected void Start() {

		buttonContinueGame.SetActive(false);
		buttonNewGame.SetActive(false);
		buttonCredits.SetActive(false);


		StartCoroutine(startAnimation());
				
		StartCoroutine(showMainTitle());

	}

	
	protected void Update() {

		if(!inputsEnabled) {
			return;
		}
		
		if(isAnyKeyPressed(KEYS_START, false) || isAnyButtonPressed(BUTTONS_START, false)) {
			StartCoroutine(showMenuButtons());
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


		yield return new WaitForSeconds(5);


		background.sprite = GameHelper.Instance.loadSpriteAsset(Constants.PATH_RES_SPLASHSCREEN + "SplashScreenBgAfter.png");

		
		for(int i = 0 ; i <= 3 ; i++) {
			
			setAlpha(foreground, (float)i / 3f);
			
			yield return new WaitForSeconds(0.05f);
		}


		yield return new WaitForSeconds(1.2f);


		StartCoroutine(hideImage(foreground));

		StartCoroutine(shakeImage(background));

		audioSourceExplosion.PlayDelayed(0.5f);

	}
	
	private IEnumerator hideImage(Image image) {
		
		for(int i = 0 ; i <= 40 ; i++) {
			
			setAlpha(image, 1 - (float)i / 40f);
			
			yield return new WaitForSeconds(0.05f);
		}

		image.color = new Color(0, 0, 0, 0);

		//stroboscopic
		while(true) {

			setAlpha(image, UnityEngine.Random.value * 0.2f);

			yield return new WaitForSeconds(0.1f);

			setAlpha(image, 0);

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
		credits.text = GAME_CREDITS;

		setAlpha(splatTitle, 0);
		setAlpha(splatCredits, 0);
		
		setAlpha(title, 0);
		setAlpha(subTitle, 0);
		setAlpha(credits, 0);

		
		yield return new WaitForSeconds(3);
		
		for(int i = 0 ; i <= 3 ; i++) {

			float value = (float)i / 3f;
			setAlpha(splatTitle, value);
			setAlpha(title, value);
			setAlpha(subTitle, value);
			
			yield return new WaitForSeconds(0.05f);
		}
		
		yield return new WaitForSeconds(0.5f);


		for(int i = 0 ; i <= 3 ; i++) {
			
			float value = (float)i / 3f;
			setAlpha(splatCredits, value);
			setAlpha(credits, value);
			
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


	//TDO improve !!!!!

	
	private bool isKeyPressed(KeyCode key, bool longPress) {
		
		/*if(!isControlsEnabled || !isControlsEnabledWhileAnimating) {
			return false;
		}*/
		if(longPress) {
			return Input.GetKey(key);
		}
		return Input.GetKeyDown(key);
	}
	
	
	private bool isAnyKeyPressed(KeyCode[] keys, bool longPress) {
		
		foreach (KeyCode k in keys) {
			if(isKeyPressed(k, longPress)) {
				return true;
			}
		}
		
		return false;
	}
	
	private bool isAllKeysPressed(KeyCode[] keys, bool longPress) {
		
		foreach (KeyCode k in keys) {
			if(!isKeyPressed(k, longPress)) {
				return false;
			}
		}
		
		return true;
	}
	
	private bool isButtonPressed(string inputControlName, bool longPress) {
		/*
		foreach(InputControl control in InputManager.ActiveDevice.Controls) {
			if(control != null && control.IsPressed) {
				Debug.Log(">>> CMD(" + control.Target + ") Handle(" + control.Handle + 
				          ") IsPressed(" + control.IsPressed + ") WasPressed(" + control.WasPressed +
				          ") WasReleased(" + control.WasReleased + ") LastValue(" + control.LastValue + ") Value(" + control.Value +
				          ") LastState(" + control.LastState + ") State(" + control.State +
				          ") HasChanged(" + control.HasChanged + ")");
			}
		}*/
		/*
		if(!isControlsEnabled || !isControlsEnabledWhileAnimating) {
			return false;
		}*/
		
		InputControl ic = InputManager.ActiveDevice.GetControlByName(inputControlName);
		
		if(!buttonPressIterations.ContainsKey(inputControlName)) {
			//register button if missing
			buttonPressIterations.Add(inputControlName, 0);
		}
		
		int iterationsCount = buttonPressIterations[inputControlName];
		
		if(!ic.State) {
			
			if(!ic.HasChanged) {
				//not pressing the button
				return false;
			}
			
			//user has just stopped pressing the button
			buttonPressIterations[inputControlName] = 0;
			
		} else {
			
			//user is still pressing the button
			buttonPressIterations[inputControlName]++;
		}
		
		
		if(longPress) {
			//long press only if there were too many iterations
			return (iterationsCount >= MAX_BUTTON_LONG_PRESS_ITERATIONS);
		}
		
		//check short press
		
		if(iterationsCount < MAX_BUTTON_LONG_PRESS_ITERATIONS && !ic.State) {
			//short press only if the max of iterations was not reached when releasing the button
			return true;
		}
		
		return false;
	}
	
	private bool isAnyButtonPressed(string[] inputControlNames, bool longPress) {
		
		foreach (string name in inputControlNames) {
			if(isButtonPressed(name, longPress)) {
				return true;
			}
		}
		
		return false;
	}
	
	private bool isAllButtonsPressed(string[] inputControlNames, bool longPress) {
		
		foreach (string name in inputControlNames) {
			if(!isButtonPressed(name, longPress)) {
				return false;
			}
		}
		
		return true;
	}


}


