using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class CharacterAnimation {

	public readonly string textureName;
	public readonly ReadOnlyCollection<CharacterAnimationKey> sortedKeys;

	/**
	 * The keys are sorted in the constructor, ex :
	 * [ CharacterAnimationKey(0, 0), CharacterAnimationKey(0.33f, 1),  CharacterAnimationKey(0.66f, 2) ]
	 */
	public CharacterAnimation(string textureName, params CharacterAnimationKey[] keys) {
		
		if(string.IsNullOrEmpty(textureName)) {
			throw new System.ArgumentException();
		}
		if(keys == null || keys.Length <= 0) {
			throw new System.ArgumentException();
		}

		this.textureName = textureName;
		
		Array.Sort(keys, delegate(CharacterAnimationKey key1, CharacterAnimationKey key2) {
			return (key1.percentage < key2.percentage) ? -1 : ((key1.percentage == key2.percentage) ? 0 : 1);
		});
		this.sortedKeys = Array.AsReadOnly(keys);

		if(sortedKeys[0].percentage != 0) {
			throw new System.ArgumentException("At least one key must have its pertentage equals to 0 : " + sortedKeys[0].percentage);
		}
	}

}

