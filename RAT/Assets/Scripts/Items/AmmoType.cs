using System;

public class AmmoType : Displayable {
	
	public static readonly AmmoType BULLET = new AmmoType("BULLET");
	public static readonly AmmoType ENERGY = new AmmoType("ENERGY");
	public static readonly AmmoType MISSILE = new AmmoType("MISSILE");

	public AmmoType(string trKey) : base("AmmoType." + trKey) {

	}

}

