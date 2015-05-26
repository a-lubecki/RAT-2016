using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeDirection : LevelNode {

		public enum Direction {
			UP,
			DOWN,
			LEFT,
			RIGHT
		}

		public Direction direction { get; private set; }

		public NodeDirection(XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				throw new System.InvalidOperationException();
			}			
			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getText() + " > 1 : " + nodeList.Count);
			}

			string nodeValue = getText(nodeList[0]);
			
			if(string.IsNullOrEmpty(nodeValue)) {
				throw new System.InvalidOperationException();
			}

			direction = (Direction)Enum.Parse(typeof(Direction), nodeValue);
		}

	}
}

