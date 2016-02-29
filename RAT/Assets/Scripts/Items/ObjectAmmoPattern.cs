using System;

public class ObjectAmmoPattern : ItemPattern {

	public ObjectAmmoPattern(string id, string trKey, int widthInBlocks, int heightInBlocks, 
	                int maxGroupable, ObjectAmmoPattern ammoType) : base(id, trKey, ItemType.OBJECT, 
	                                            ItemSubType.OBJECT_AMMO, widthInBlocks, heightInBlocks,
	                                            true, maxGroupable, null) {

	}

}

