using System;
using System.Xml;

namespace Level {

	public abstract class LevelNode {

		private XmlNode node;

		public LevelNode(XmlNode node) {
			if(node == null) {
				throw new System.InvalidOperationException();
			}
			this.node = node;
		}
		
		protected string getNodeId() {
			return getNodeId(node);
		}
		
		protected string getNodeText() {
			return getNodeText(node);
		}
		
		public static string getNodeId(XmlNode node) {
			return getAttributeValue(node, "ID");
		}

		public static string getNodeText(XmlNode node) {
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

