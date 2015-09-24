using UnityEngine;
using System.Collections;
using Level;

public abstract class Character : MonoBehaviour {
	
	private int _maxLife;
	public int maxLife { 
		get {
			return _maxLife;
		}
		set {
			if(value <= 0) {
				_maxLife = 0;
			} else {
				_maxLife = value;
			}
			updateViews();
		}
	}
	
	private int _life;
	public int life { 
		get {
			return _life;
		}
		set {
			if(value <= 0) {
				_life = 0;
			} else if(value > _maxLife) {
				_life = _maxLife;
			} else {
				_life = value;
			}
			updateViews();
		}
	}

	private bool isTemporaryInvulnerable = false;

	public bool isDead() {
		return (life <= 0);
	}

	public void takeDamages(int damages) {

		if(life <= 0) {
			//already dead
			return;
		}

		if(damages == 0) {
			//not a heal or a damage
			return;
		}

		if(!isTemporaryInvulnerable) {

			life -= damages;

			//set invulnerable to avoid taking all life in few milliseconds
			StartCoroutine(setTemporaryInvulnerable());

			if(life <= 0) {
				die();
			}
		}
		
	}
	
	IEnumerator setTemporaryInvulnerable() {
		
		isTemporaryInvulnerable = true;

		yield return new WaitForSeconds(0.5f);

		isTemporaryInvulnerable = false;
	}
	
	protected virtual void respawn() {
		
		//remove all colliders
		foreach(Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = true;
		}

	}

	protected virtual void die() {

		setAsDead();
	}
	
	protected virtual void setAsDead() {
		
		//remove all colliders
		foreach(Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = false;
		}
	}
	
	protected abstract void updateViews();


}



