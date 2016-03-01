using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface InputsManagerListener {

	void onActionDone(AbstractInputAction action);
}

// controller buttons mapping : http://www.gallantgames.com/pages/incontrol-standardized-controls
public class InputsManager : MonoBehaviour {
	
	private static InputsManager instance;
	
	private InputsManager() {}
	
	public static InputsManager Instance {
		get {
			return GameHelper.Instance.getInputsManager();
		}
	}


	public bool isPaused { get; private set; }


	private HashSet<AbstractInputAction> possibleActions = new HashSet<AbstractInputAction>();

	private InputActionPlayerRun _inputActionPlayerRun;
	public InputActionPlayerRun inputActionPlayerRun {
		get {
			return _inputActionPlayerRun;
		}
		private set {
			_inputActionPlayerRun = value;
		}
	}

	private InputActionPlayerMove _inputActionPlayerMove;
	public InputActionPlayerMove inputActionPlayerMove {
		get {
			return _inputActionPlayerMove;
		}
		private set {
			_inputActionPlayerMove = value;
		}
	}

	void Start() {

		int sceneId = SceneManager.GetActiveScene().buildIndex;
		if(sceneId == (int)(Constants.SceneIndex.SCENE_INDEX_SPLASHSCREEN)) {

			possibleActions.Add(new InputActionSplashscreenStart());
			//TODO

		} else if(sceneId == (int)(Constants.SceneIndex.SCENE_INDEX_LEVEL)) {
			
			possibleActions.Add(new InputActionMenuCancel());
			possibleActions.Add(new InputActionMenuNavigateDown());
			possibleActions.Add(new InputActionMenuNavigateLeft());
			possibleActions.Add(new InputActionMenuNavigateRight());
			possibleActions.Add(new InputActionMenuNavigateUp());
			possibleActions.Add(new InputActionMenuOpenClose());
			possibleActions.Add(new InputActionMenuValidate());
			
			possibleActions.Add(new InputActionSubMenuNext());
			possibleActions.Add(new InputActionSubMenuPrevious());

			possibleActions.Add(new InputActionPlayerActivate());
			possibleActions.Add(new InputActionPlayerAttackLeft());
			possibleActions.Add(new InputActionPlayerAttackRight());
			possibleActions.Add(new InputActionPlayerChangeHeal());
			possibleActions.Add(new InputActionPlayerChangeObject());
			possibleActions.Add(new InputActionPlayerChangeWeaponLeft());
			possibleActions.Add(new InputActionPlayerChangeWeaponRight());
			possibleActions.Add(new InputActionPlayerDash());
			possibleActions.Add(new InputActionPlayerUseHeal());
			possibleActions.Add(new InputActionPlayerUseObject());
			
			_inputActionPlayerRun = new InputActionPlayerRun();
			possibleActions.Add(_inputActionPlayerRun);

			//create move action but don't add it to the posssible actions
			_inputActionPlayerMove = new InputActionPlayerMove();

		}

	}
	
	
	protected void OnApplicationFocus(bool focusStatus) {
		this.isPaused = !focusStatus;
	}

	
	void Update() {
		
		if(isPaused) {
			return;
		}

		foreach(AbstractInputAction action in possibleActions) {
			if(action.isActionDone()) {
				action.execute();
			}
		}

	}
	
	void FixedUpdate() {
		
		if(isPaused) {
			return;
		}

		if(_inputActionPlayerMove != null) {
			if(_inputActionPlayerMove.isActionDone()) {
				_inputActionPlayerMove.execute();
			}
		}
	}


}

