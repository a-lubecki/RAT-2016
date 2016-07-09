using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {


	private int _skillPointsHealth;
	public int skillPointsHealth { 
		get {
			return _skillPointsHealth;
		}
		set {
			if(value < 1) {
				_skillPointsHealth = 1;
			} else {
				_skillPointsHealth = value;
			}
		}
	}

	private int _skillPointsEnergy;
	public int skillPointsEnergy { 
		get {
			return _skillPointsEnergy;
		}
		set {
			if(value < 1) {
				_skillPointsEnergy = 1;
			} else {
				_skillPointsEnergy = value;
			}
		}
	}

	private int _maxStamina;
	public int maxStamina { 
		get {
			return _maxStamina;
		}
		set {
			if(value <= 0) {
				_maxStamina = 0;
			} else {
				_maxStamina = value;
			}
		}
	}

	private int _stamina;
	public int stamina { 
		get {
			return _stamina;
		}
		set {
			if(value <= 0) {
				_stamina = 0;
			} else if(value > _maxStamina) {
				_stamina = _maxStamina;
			} else {
				_stamina = value;
			}
		}
	}

	private int _xp;
	public int xp { 
		get {
			return _xp;
		}
		set {
			if(value <= 0) {
				_xp = 0;
			} else {
				_xp = value;
			}
		}
	}

	public string levelNameForLastHub;


	public Player(int skillPointsHealth, int skillPointsEnergy, bool setLife, int life, bool setStamina, int stamina, int xp, int angleDegrees) 
		: base(Constants.GAME_OBJECT_NAME_PLAYER, null, 0, 0, 0, 0, CharacterDirection.UP, angleDegrees) {

		this.skillPointsHealth = skillPointsHealth;
		this.skillPointsEnergy = skillPointsEnergy;

		//compute stats
		this.maxLife = 50 + 5 * skillPointsHealth + 2 * skillPointsEnergy;
		this.maxStamina = 80 + 5 * skillPointsEnergy;

		if(setLife) {
			this.life = life;
		} else {
			this.life = maxLife;
		}
		if(setStamina) {
			this.stamina = stamina;
		} else {
			this.stamina = maxStamina;
		}

		this.xp = xp;

	}

	public void reinitLifeAndStamina() {

		this.life = maxLife;
		this.stamina = maxStamina;
	}

	public void earnXp(int newXp) {

		if(newXp <= 0) {
			throw new ArgumentException();
		}

		this.xp += newXp;

	}


	public void enableControls(object caller) {

		//TODO refaire en bien

		GameObject gameObject = findGameObject<PlayerBehavior>();

		if (gameObject == null) {
			throw new InvalidOperationException();
		}

		PlayerBehavior playerBehavior = gameObject.GetComponent<PlayerBehavior>();

		playerBehavior.enableControls();
	}

	public void disableControls(object caller) {

		//TODO refaire en bien

		GameObject gameObject = findGameObject<PlayerBehavior>();

		if (gameObject == null) {
			throw new InvalidOperationException();
		}

		PlayerBehavior playerBehavior = gameObject.GetComponent<PlayerBehavior>();

		playerBehavior.disableControls();
	}

}

