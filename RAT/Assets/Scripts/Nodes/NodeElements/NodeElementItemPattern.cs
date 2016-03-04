using System;
using System.Xml;
using UnityEngine;

namespace Node {

	public class NodeElementItemPattern : BaseNodeElement {

		public NodeInt nodeMaxGroupable { get ; private set; }
		public NodeInt nodeWidth { get ; private set; }
		public NodeInt nodeHeight { get ; private set; }
		public NodeLabel nodeAmmoPattern { get ; private set; }

		public NodeElementItemPattern(XmlNode node) : base(node) {

			nodeMaxGroupable = parseChild("maxGroupable", typeof(NodeInt)) as NodeInt;
			nodeWidth = parseChild("width", typeof(NodeInt)) as NodeInt;
			nodeHeight = parseChild("height", typeof(NodeInt)) as NodeInt;
			nodeAmmoPattern = parseChild("ammoPattern", typeof(NodeLabel)) as NodeLabel;

			if(nodeMaxGroupable == null) {
				nodeMaxGroupable = new NodeInt(1);
			} else if(nodeMaxGroupable.value < 1) {
				throw new System.InvalidOperationException("Max groupable must be 1 or more : " + nodeMaxGroupable.value);
			}

			if(nodeWidth == null) {
				nodeWidth = new NodeInt(1);
			} else if(nodeWidth.value < 1) {
				throw new System.InvalidOperationException("Width must be 1 or more : " + nodeWidth.value);
			}

			if(nodeHeight == null) {
				nodeHeight = new NodeInt(1);
			} else if(nodeHeight.value < 1) {
				throw new System.InvalidOperationException("Height must be 1 or more : " + nodeHeight.value);
			}

		}
		
		public override void freeXmlObjects() {

			nodeMaxGroupable.freeXmlObjects();
			nodeWidth.freeXmlObjects();
			nodeHeight.freeXmlObjects();
			if(nodeAmmoPattern != null) {
				nodeAmmoPattern.freeXmlObjects();
			}
			
			base.freeXmlObjects();
		}

	}
}

