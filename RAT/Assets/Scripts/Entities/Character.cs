using UnityEngine;
using System.Collections;
using Level;

public abstract class Character : MonoBehaviour {
	
	public int maxLife { get; protected set; }
	public int life { get; protected set; }

	private bool isTemporaryInvulnerable = false;

	public virtual void init() {
		
		this.maxLife = 1;
		this.life = 1;
		
		updateViews();
	}

	public bool isDead() {
		return (life <= 0);
	}

	public void setMaxLife(int maxLife) {

		if(maxLife <= 0) {
			this.maxLife = 1;
		} else {
			this.maxLife = maxLife;
		}

		updateViews();
	}

	public void takeDamages(int damages) {

		if(life <= 0) {
			//already dead
			return;
		}

		if(damages == 0) {
			return;
		}

		if(!isTemporaryInvulnerable) {

			life -= damages;

			if(life < 0) {
				life = 0;
			} else if(life > maxLife) {
				life = maxLife;
			}

			updateViews();

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
		
	protected virtual void die() {
		
		//remove all colliders
		foreach(Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = false;
		}
		
	}
	
	protected abstract void updateViews();


}



