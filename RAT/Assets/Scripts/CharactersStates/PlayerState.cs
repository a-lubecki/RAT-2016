using System;

public class PlayerState : BaseCharacterState {
	public static readonly PlayerState DASH = new PlayerState();
	public static readonly PlayerState SHORT_ATTACK = new PlayerState();
	public static readonly PlayerState HEAVY_ATACK = new PlayerState();
	public static readonly PlayerState OBJECT_USE = new PlayerState();
}
