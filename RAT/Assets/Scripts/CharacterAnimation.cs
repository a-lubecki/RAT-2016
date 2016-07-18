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

	public override bool Equals (object obj) {
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(CharacterAnimation))
			return false;
		
		CharacterAnimation other = (CharacterAnimation)obj;
		if (!textureName.Equals(other.textureName)) {
			return false;
		}
		if (sortedKeys.Count != other.sortedKeys.Count) {
			return false;
		}
		for (int i = 0; i < sortedKeys.Count; i++) {
			if (!sortedKeys[i].Equals(other.sortedKeys[i])) {
				return false;
			}
		}
		return true;
	}
	

	public override int GetHashCode () {
		unchecked {
			return (textureName != null ? textureName.GetHashCode () : 0) ^ (sortedKeys != null ? sortedKeys.GetHashCode () : 0);
		}
	}
	

}

