using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class Position : LevelNode {

		public int x { get; private set; }
		public int y { get; private set; }
		
		public Position(XmlNode node) : base(node) {

			XmlNodeList nodeList = node.SelectNodes("node");
			if(nodeList.Count <= 0) {
				return;
			}
			
			XmlNode nodeX = nodeList[0];
			string strX = getNodeText(nodeX);
			if(!String.IsNullOrEmpty(strX)) {
				x = int.Parse(strX);
			}

			if(nodeList.Count <= 1) {
				return;
			}

			XmlNode nodeY = nodeList[1];
			string strY = getNodeText(nodeY);
			if(!String.IsNullOrEmpty(strY)) {
				y = int.Parse(strY);
			}

			
			if(nodeList.Count > 1) {
				Debug.LogWarning("Nb elements for " + getNodeText() + " > 2 : " + nodeList.Count);
			}

		}


	}

}

