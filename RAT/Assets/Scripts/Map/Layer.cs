using System;
using System.Collections;
using System.Collections.Generic;

namespace TiledMap {

	public class Layer {
		
		public string sortingLayerName { get; private set; }
		public int orderInLayer { get; private set; }

		private int x, y, width, height;

		public int nbElemsX { get; private set; }
		public int nbElemsY { get; private set; }

		private Tile[,] tiles;

		public Layer(Map map, Dictionary<string, object> dict) {

			string name = (string)dict["name"];
			if(String.IsNullOrEmpty(name)) {
				throw new System.InvalidOperationException();
			}

			if(name.Contains("_")) {

				string[] parts = name.Split(new string[] {"_"}, StringSplitOptions.RemoveEmptyEntries);

				if(parts.Length != 2) {
					throw new System.InvalidOperationException();
				}

				sortingLayerName = parts[0];
				orderInLayer = int.Parse(parts[1]);

			} else {
				sortingLayerName = name;
			}


			x = (int)(long)dict["x"];
			y = (int)(long)dict["y"];
			width = (int)(long)dict["width"];
			height = (int)(long)dict["height"];

			if(x < 0) {
				throw new System.InvalidOperationException();
			}
			if(y < 0) {
				throw new System.InvalidOperationException();
			}
			if(width <= 0) {
				throw new System.InvalidOperationException();
			}
			if(height <= 0) {
				throw new System.InvalidOperationException();
			}

			nbElemsX = x + width;
			nbElemsY = y + height;

			//load tiles
			tiles = new Tile[height, width];

			List<object> dataJson = (List<object>)dict["data"];

			int ix = 0, iy = 0;
			foreach (object t in dataJson) {
				
				int id = (int)(long)t;

				if(id > 0) {

					TileDescriptor tileDescriptor = map.getTileDescriptor(id);

					if(tileDescriptor != null) {
						tiles[iy, ix] = new Tile(id, x+ix, y+iy, tileDescriptor);
					}
				}

				ix++;

				if(ix >= width) {
					ix = 0;
					iy++;
				}
			}

		}
		
		public Tile getTileAt(int posx, int posy) {

			if(posx < 0) {
				throw new System.ArgumentException();
			}			
			if(posx >= nbElemsX) {
				throw new System.ArgumentException();
			}
			if(posy < 0) {
				throw new System.ArgumentException();
			}			
			if(posy >= nbElemsY) {
				throw new System.ArgumentException();
			}
			
			if(posx < x) {
				return null;
			}
			if(posx >= x + width) {
				return null;
			}
			if(posy < y) {
				return null;
			}
			if(posy >= y + height) {
				return null;
			}

			return tiles[posy, posx];
		}

	}
}

