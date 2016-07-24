using System;

public class BaseCharacterState {
	
	public static readonly BaseCharacterState UNDEFINED = new BaseCharacterState();
	public static readonly BaseCharacterState WAIT = new BaseCharacterState();
	public static readonly BaseCharacterState WALK = new BaseCharacterState();
	public static readonly BaseCharacterState RUN = new BaseCharacterState();
	public static readonly BaseCharacterState HURT = new BaseCharacterState();
	public static readonly BaseCharacterState DEATH = new BaseCharacterState();

	public override string ToString() {

		if (this == UNDEFINED) {
			return "UNDEFINED";
		}
		if (this == WAIT) {
			return "WAIT";
		}
		if (this == WALK) {
			return "WALK";
		}
		if (this == RUN) {
			return "RUN";
		}
		if (this == HURT) {
			return "HURT";
		}
		if (this == DEATH) {
			return "DEATH";
		}

		throw new InvalidOperationException();
	}

}
