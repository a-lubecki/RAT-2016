using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeCharacterDirection : BaseNode {

		public CharacterDirection value { get; private set; }

		public NodeCharacterDirection() {
			value = CharacterDirection.UP;
		}

		public NodeCharacterDirection(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();

			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getText() + " > 1 : " + nodeList.Count);
			}

			string nodeValue = getText(nodeList[0]);

			if(string.IsNullOrEmpty(nodeValue)) {
				throw new System.InvalidOperationException();
			}

			value = (CharacterDirection)Enum.Parse(typeof(CharacterDirection), nodeValue);
		}

	}
}

