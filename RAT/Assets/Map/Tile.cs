using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMap {
	
	public class Tile {
		
		public int id { get; private set; }

		public int x { get; private set; }
		public int y { get; private set; }

		public TileDescriptor tileDescriptor { get; private set; }
		//public Sprite tileSprite { get; private set; }

		public Tile(int id, int x, int y, TileDescriptor tileDescriptor) {

			this.id = id;

			this.x = x;
			this.y = y;

			this.tileDescriptor = tileDescriptor;
		}


	}
}

