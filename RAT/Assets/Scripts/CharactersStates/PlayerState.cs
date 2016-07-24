using System;

public class PlayerState : BaseCharacterState {
	
	public static readonly PlayerState DASH = new PlayerState();
	public static readonly PlayerState SHORT_ATTACK = new PlayerState();
	public static readonly PlayerState HEAVY_ATTACK = new PlayerState();
	public static readonly PlayerState DISTANCE_ATTACK = new PlayerState();
	public static readonly PlayerState OBJECT_USE = new PlayerState();
	public static readonly PlayerState DEFEND = new PlayerState();


	public override string ToString() {

		if (this == DASH) {
			return "DASH";
		}
		if (this == SHORT_ATTACK) {
			return "SHORT_ATTACK";
		}
		if (this == HEAVY_ATTACK) {
			return "HEAVY_ATTACK";
		}
		if (this == DISTANCE_ATTACK) {
			return "DISTANCE_ATTACK";
		}
		if (this == OBJECT_USE) {
			return "OBJECT_USE";
		}
		if (this == DEFEND) {
			return "DEFEND";
		}

		return base.ToString();
	}
}
