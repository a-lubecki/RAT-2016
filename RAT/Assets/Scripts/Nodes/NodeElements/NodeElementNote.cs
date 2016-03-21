using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace Node {

	public class NodeElementNote : BasePositionableElement {

		private List<string> textKeyElements = new List<string>();

		public NodeElementNote (XmlNode node) : base(node) {

			XmlNodeList nodeList = getNodeChildren();

			foreach(XmlNode n in nodeList) {

				string l = getText(n);

				if("texts".Equals(l)) {

					XmlNodeList nodeList2 = getNodeChildren(n);

					foreach(XmlNode n2 in nodeList2) {

						string str = getText(n2);
						if(String.IsNullOrEmpty(str)) {
							throw new InvalidOperationException("Text null or empty for Note");
						}

						textKeyElements.Add(str);
					}

				}
			}

		}

		public int getTextCount() {
			return textKeyElements.Count;
		}

		public string getText(int pos) {

			if(pos < 0 || pos >= textKeyElements.Count) {
				throw new ArgumentException();
			}

			return textKeyElements[pos];
		}

	}
}

