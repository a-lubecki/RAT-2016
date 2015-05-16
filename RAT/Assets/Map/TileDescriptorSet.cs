using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMap {

	public class TileDescriptorSet {
		
		public string name { get; private set; }
		public Texture2D image { get; private set; }
		
		public int nbElemsX { get; private set; }
		public int nbElemsY { get; private set; }
		private TileDescriptor[,] tileDescriptors;

		public TileDescriptorSet(Map map, Dictionary<string, object> dict) {

			//load name
			name = (string)dict["name"];

			//load texture
			string imagePath = (string)dict["image"];
			if(String.IsNullOrEmpty(imagePath)) {
				throw new System.InvalidOperationException();
			}

			image = (Texture2D)Resources.LoadAssetAtPath("Assets/Res/" + imagePath, typeof(Texture2D));

			if(image == null) {
				throw new System.ArgumentException("Wrong image path : " + imagePath);
			}
			
			int imageWidth = (int)(long)dict["imagewidth"];
			if(imageWidth <= 0) {
				throw new System.InvalidOperationException();
			}

			int imageHeight = (int)(long)dict["imageheight"];
			if(imageHeight <= 0) {
				throw new System.InvalidOperationException();
			}
			
			int pixelsPerUnit = (int)(long)dict["tilewidth"];//32
			if(pixelsPerUnit <= 0) {
				throw new System.InvalidOperationException();
			}

			int pixelsPerUnit2 = (int)(long)dict["tileheight"];//32
			if(pixelsPerUnit != pixelsPerUnit2) {
				throw new System.InvalidOperationException("tile width and height must be the same (pixelsPerUnit)");
			}

			//create tiles descriptor
			
			int id = (int)(long)dict["firstgid"];

			nbElemsX = (int)(imageWidth / (float)pixelsPerUnit);
			nbElemsY = (int)(imageHeight / (float)pixelsPerUnit);

			tileDescriptors = new TileDescriptor[nbElemsY, nbElemsX];

			for(int y = 0 ; y < nbElemsY ; y++) {
				for(int x = 0 ; x < nbElemsX ; x++) {

					TileDescriptor tileDescriptor = new TileDescriptor(id++, image, x, nbElemsY - 1 - y, pixelsPerUnit);
					tileDescriptors[y, x] = tileDescriptor;
					map.registerTileDescriptor(tileDescriptor);
				}
			}


			//TODO load animations

		}
	}
}

