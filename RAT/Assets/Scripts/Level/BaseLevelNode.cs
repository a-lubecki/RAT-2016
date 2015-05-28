using System;
using System.Xml;
using System.Collections.Generic;

namespace Level {

	public abstract class BaseLevelNode {

		private XmlNode node;
		
		public BaseLevelNode() {
		}

		public BaseLevelNode(XmlNode node) {

			if(node == null) {
				throw new System.InvalidOperationException();
			}
			this.node = node;

			XmlNodeList nodeList = getNodeChildren();
			if(nodeList.Count <= 0) {
				throw new System.InvalidOperationException();
			}
		}

		protected XmlNodeList getNodeChildren() {
			return getNodeChildren(node);
		}

		protected string getId() {
			return getId(node);
		}
		
		protected string getText() {
			return getText(node);
		}

		public static XmlNodeList getNodeChildren(XmlNode node) {
			return node.SelectNodes("node");
		}

		public static string getId(XmlNode node) {
			return getAttributeValue(node, "ID");
		}

		public static string getText(XmlNode node) {
			return getAttributeValue(node, "TEXT");			
		}
		
		public static string getAttributeValue(XmlNode node, string attributeName) {
			
			if(node == null) {
				throw new System.InvalidOperationException();
			}
			
			XmlAttributeCollection attributes = node.Attributes;
			
			if(attributes == null) {
				return null;
			}

			XmlAttribute attribute = attributes[attributeName];
			
			if(attribute == null) {
				return null;
			}
			
			string val = attribute.Value;
			if(String.IsNullOrEmpty(val)) {
				throw new System.InvalidOperationException();
			}
			
			return val;
		}
		
		protected BaseLevelNode parseChild(string label, Type fieldType) {
			return parseChild(label, fieldType, false);
		}

		protected BaseLevelNode parseChild(string label, Type fieldType, bool returnNonNull) {

			if(string.IsNullOrEmpty(label)) {
				throw new ArgumentException();
			}

			XmlNodeList nodeList = getNodeChildren();

			foreach(XmlNode n in nodeList) {
				
				string l = getText(n);
				
				if(label.Equals(l)) {
					return newLevelNode(fieldType, n);
				}
			}

			if(returnNonNull) {
				return newLevelNode(fieldType);
			}

			return null;
		}
			
		protected List<BaseLevelNode> parseChildren(string label, Type fieldType) {
			
			if(string.IsNullOrEmpty(label)) {
				throw new ArgumentException();
			}

			List<BaseLevelNode> res = new List<BaseLevelNode>();

			XmlNodeList nodeList = getNodeChildren();

			foreach(XmlNode n in nodeList) {
				
				string l = getText(n);
				
				if(label.Equals(l)) {
					res.Add(newLevelNode(fieldType, n));
				}
			}

			return res;
		}
		

		private BaseLevelNode newLevelNode(Type fieldType, params XmlNode[] args) {

			return Activator.CreateInstance(fieldType, args) as BaseLevelNode;
		}

	}
}

