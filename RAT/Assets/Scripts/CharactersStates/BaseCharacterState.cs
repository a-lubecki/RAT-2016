using System;

public class BaseCharacterState {
	public static readonly BaseCharacterState UNDEFINED = new BaseCharacterState();
	public static readonly BaseCharacterState WAIT = new BaseCharacterState();
	public static readonly BaseCharacterState WALK = new BaseCharacterState();
	public static readonly BaseCharacterState RUN = new BaseCharacterState();
	public static readonly BaseCharacterState HURT = new BaseCharacterState();
	public static readonly BaseCharacterState DEATH = new BaseCharacterState();
}
