using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMap {

	public class TileDescriptor {

		public int id { get; private set; }
		public Sprite tileSprite { get; private set; }

		public TileDescriptor(int id, Texture2D texture, int x, int y, int pixelsPerUnit) {
			this.id = id;
			tileSprite = Sprite.Create(
				texture, 
				new Rect(x * pixelsPerUnit, y * pixelsPerUnit, pixelsPerUnit, pixelsPerUnit), 
				new Vector2(0.5f, 0.5f), 
				pixelsPerUnit);
		}

	}
}

