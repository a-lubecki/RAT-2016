using System;
using System.Xml;
using System.Collections.Generic;

namespace Node {

	public class BaseNode {

		private XmlNode node;
		
		public BaseNode() {
		}

		public BaseNode(XmlNode node) {

			if(node == null) {
				throw new System.ArgumentException();
			}
			this.node = node;
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

		public static string getText(BaseNode baseNode) {
			return getText(baseNode.node);
		}
		
		public static string getAttributeValue(XmlNode node, string attributeName) {
			
			if(node == null) {
				throw new System.ArgumentException();
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
		
		public BaseNode parseChild(string label, Type fieldType) {
			return parseChild(label, fieldType, false);
		}

		public BaseNode parseChild(string label, Type fieldType, bool returnNonNull) {

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
			
		public List<BaseNode> parseChildren(string label, Type fieldType) {
			
			if(string.IsNullOrEmpty(label)) {
				throw new ArgumentException();
			}

			List<BaseNode> res = new List<BaseNode>();

			XmlNodeList nodeList = getNodeChildren();

			foreach(XmlNode n in nodeList) {
				
				string l = getText(n);
				
				if(label.Equals(l)) {
					res.Add(newLevelNode(fieldType, n));
				}
			}

			return res;
		}

		public List<BaseNode> parseAllChildren(Type fieldType) {

			List<BaseNode> res = new List<BaseNode>();

			XmlNodeList nodeList = getNodeChildren();

			foreach(XmlNode n in nodeList) {
				res.Add(newLevelNode(fieldType, n));
			}

			return res;
		}


		private BaseNode newLevelNode(Type fieldType, params XmlNode[] args) {

			return Activator.CreateInstance(fieldType, args) as BaseNode;
		}

		/**
		 * When the xml object is not used anymore, free it
		 */
		public virtual void freeXmlObjects() {

			node = null;
		}
	}
}

