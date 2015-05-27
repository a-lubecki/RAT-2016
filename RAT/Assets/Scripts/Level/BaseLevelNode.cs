using System;
using System.Xml;

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

	}
}

