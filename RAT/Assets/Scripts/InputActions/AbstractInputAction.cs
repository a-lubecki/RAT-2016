using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public abstract class AbstractInputAction {
	
	private static readonly int MAX_BUTTON_LONG_PRESS_ITERATIONS = 30;
	private Dictionary<string, int> buttonPressIterations = new Dictionary<string, int>();
	
	public virtual bool areActionKeysLongPressed() {
		return false;
	}
	public abstract KeyCode[] getDefaultActionKeys();
	
	public virtual bool areActionButtonsLongPressed() {
		return false;
	}
	public abstract string[] getDefaultActionButtons();
	
	public abstract bool execute();


	public virtual bool isActionDone() {
		return (isAnyKeyPressed(getDefaultActionKeys(), areActionKeysLongPressed()) ||
		        isAnyButtonPressed(getDefaultActionButtons(), areActionButtonsLongPressed()));
	}


	protected bool isKeyPressed(KeyCode key, bool longPress) {

		if(longPress) {
			return Input.GetKey(key);
		}
		return Input.GetKeyDown(key);
	}
	
	
	protected bool isAnyKeyPressed(KeyCode[] keys, bool longPress) {
		
		foreach (KeyCode k in keys) {
			if(isKeyPressed(k, longPress)) {
				return true;
			}
		}
		
		return false;
	}
	
	protected bool isAllKeysPressed(KeyCode[] keys, bool longPress) {
		
		foreach (KeyCode k in keys) {
			if(!isKeyPressed(k, longPress)) {
				return false;
			}
		}
		
		return true;
	}
	
	protected bool isButtonPressed(string inputControlName, bool longPress) {
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
	
	protected bool isAnyButtonPressed(string[] inputControlNames, bool longPress) {
		
		foreach (string name in inputControlNames) {
			if(isButtonPressed(name, longPress)) {
				return true;
			}
		}
		
		return false;
	}
	
	protected bool isAllButtonsPressed(string[] inputControlNames, bool longPress) {
		
		foreach (string name in inputControlNames) {
			if(!isButtonPressed(name, longPress)) {
				return false;
			}
		}
		
		return true;
	}

}

