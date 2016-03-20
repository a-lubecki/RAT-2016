using System;
using System.Collections.Generic;

public class Character : BaseIdentifiableModel {

	public int initialPosX { get ; private set; }
	public int initialPosY { get ; private set; }
	public CharacterDirection initialDirection { get ; private set; }

	private int angleDegrees_;
	public int angleDegrees { 
		get { 
			return angleDegrees_;
		} 
		set {
			value = value % 360;

			if(value < 0) {
				value += 360;
			}

			angleDegrees_ = value;
		}
	}

	public bool isMoving { get; set; }
	public bool isRunning { get; set; }

	public BaseCharacterState currentState { get; private set; }

	public bool isInvulnerable { get; set; }

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
		}
	}


	public Character(string id, List<Listener> listeners, int maxLife, int life, int initialPosX, int initialPosY, CharacterDirection initialDirection, int angleDegrees) 
		: base(id, listeners) {

		this.maxLife = maxLife;
		this.life = life;
		this.initialPosX = initialPosX;
		this.initialPosY = initialPosY;
		this.initialDirection = initialDirection;
		this.angleDegrees = angleDegrees;
	}


	public bool isDead() {
		return (life <= 0);
	}


	public void takeDamages(int damages) {

		if(damages < 0) {
			throw new ArgumentException("Can't take negative damages");
		}

		if(isInvulnerable) {
			return;
		}
		if(damages == 0) {
			//not a heal or a damage
			return;
		}
		if(isDead()) {
			//already dead
			return;
		}

		life -= damages;
	}

	public void heal(int heal) {

		if(heal < 0) {
			throw new ArgumentException("Can't take negative heal");
		}
		if(heal == 0) {
			//not a heal or a damage
			return;
		}

		if(isDead()) {
			//already dead
			return;
		}

		life += heal;
	}


	public void setAsDead() {

		life = 0;
	}

	public void changeState(BaseCharacterState state) {

		if(state == BaseCharacterState.UNDEFINED) {
			return;
		}

		currentState = state;
	}

	public CharacterDirection getCharacterDirection(CharacterDirection currentDirection, int halfAngle) {

		if(currentDirection == CharacterDirection.RIGHT ||
			currentDirection == CharacterDirection.LEFT) {

			if(isCharacterDirectionRight(angleDegrees, halfAngle)) {
				return CharacterDirection.RIGHT;
			} 
			if(isCharacterDirectionLeft(angleDegrees, halfAngle)) {
				return CharacterDirection.LEFT;
			} 
			if(isCharacterDirectionUp(angleDegrees, halfAngle)) {
				return CharacterDirection.UP;
			} 
			if(isCharacterDirectionDown(angleDegrees, halfAngle)) {
				return CharacterDirection.DOWN;
			}
		}

		if(currentDirection == CharacterDirection.UP ||
			currentDirection == CharacterDirection.DOWN) {

			if(isCharacterDirectionUp(angleDegrees, halfAngle)) {
				return CharacterDirection.UP;
			} 
			if(isCharacterDirectionDown(angleDegrees, halfAngle)) {
				return CharacterDirection.DOWN;
			} 
			if(isCharacterDirectionRight(angleDegrees, halfAngle)) {
				return CharacterDirection.RIGHT;
			} 
			if(isCharacterDirectionLeft(angleDegrees, halfAngle)) {
				return CharacterDirection.LEFT;
			}
		}

		return CharacterDirection.DOWN;
	}

	private static bool isCharacterDirectionUp(float angle, int halfAngle) {
		return (360 - halfAngle <= angle || angle <= halfAngle);
	}
	private static bool isCharacterDirectionRight(float angle, int halfAngle) {
		return (90 - halfAngle <= angle && angle <= 90 + halfAngle);
	}
	private static bool isCharacterDirectionDown(float angle, int halfAngle) {
		return (180 - halfAngle <= angle && angle <= 180 + halfAngle);
	}
	private static bool isCharacterDirectionLeft(float angle, int halfAngle) {
		return (270 - halfAngle <= angle && angle <= 270 + halfAngle);
	}

	public static int directionToAngle(CharacterDirection direction) {

		if(direction == CharacterDirection.RIGHT) {
			return 90;
		} 
		if(direction == CharacterDirection.LEFT) {
			return -90;
		} 
		if(direction == CharacterDirection.UP) {
			return 0;
		}
		return 180;
	}

}

