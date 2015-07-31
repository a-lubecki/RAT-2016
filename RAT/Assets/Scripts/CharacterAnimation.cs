using System;
using System.Collections.ObjectModel;

public class CharacterAnimation {

	public readonly bool isBlocking;
	public readonly string textureName;
	public readonly ReadOnlyCollection<CharacterAnimationKey> keys;

	public CharacterAnimation(bool isBlocking, string textureName, params CharacterAnimationKey[] keys) {

		if(keys == null || keys.Length <= 0) {
			throw new System.ArgumentException();
		}
		if(string.IsNullOrEmpty(textureName)) {
			throw new System.ArgumentException();
		}

		this.isBlocking = isBlocking;
		this.textureName = textureName;
		this.keys = Array.AsReadOnly(keys);
	}

}

