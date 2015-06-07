using System;
using System.Xml;
using UnityEngine;

namespace Level {

	public class NodeString : NodeLabel {

		private static char TAG = '\"';

		public NodeString(XmlNode node) : base(node) {
			
			value = decodeHtmlText(value);

			if(value.Length < 2) {
				throw new System.InvalidOperationException();
			}

			//remove first and last quote characters

			char c0 = value[0];
			if(!c0.Equals(TAG)) {
				throw new System.InvalidOperationException();
			}
			char cn = value[value.Length-1];
			if(!cn.Equals(TAG)) {
				throw new System.InvalidOperationException();
			}
			
			value = value.Substring(1, value.Length-2);

		}

		private string decodeHtmlText(string text) {
			
			string[] parts = text.Split(new string[] { "&#x" }, StringSplitOptions.None);

			for (int i = 1; i < parts.Length; i++) {

				int n = parts[i].IndexOf(';');
				string number = parts[i].Substring(0, n);

				try {
					int unicode = Convert.ToInt32(number, 16);
					parts[i] = ((char)unicode) + parts[i].Substring(n + 1);
				} catch {}
			}
			return String.Join("", parts);
		}

	}

}

