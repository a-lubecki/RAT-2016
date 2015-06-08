using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMap {
	
	public class Tile {
		
		public long id { get; private set; }

		public Quaternion rotation { get; private set; }

		public int x { get; private set; }
		public int y { get; private set; }

		public TileDescriptor tileDescriptor { get; private set; }

		public Tile(long id, int x, int y, TileDescriptor tileDescriptor) {

			this.id = id;

			this.rotation = getRotation(id);

			this.x = x;
			this.y = y;

			this.tileDescriptor = tileDescriptor;
		}


		public static int getTileDescriptorId(long id) {

			//convert to binary
			BitArray bitArray = new BitArray(new [] { (int)id });

			//rotation is coded on the first 3 bits : remove them
			BitArray rootIdFilter = bitArray.Clone() as BitArray;
			rootIdFilter.Set(29, false);
			rootIdFilter.Set(30, false);
			rootIdFilter.Set(31, false);
			
			//filter to get id by removing the rotation bits
			int[] resArray = new int[1];
			bitArray.And(rootIdFilter).CopyTo(resArray, 0);
			return resArray[0];
		}

		
		private static Quaternion getRotation(long id) {

			//convert to binary
			BitArray bitArray = new BitArray(new [] { (int)id });
			
			//rotation is coded on the first 3 bits : get them
			bool bit29 = bitArray.Get(29);
			bool bit30 = bitArray.Get(30);
			bool bit31 = bitArray.Get(31);

			return Quaternion.Euler(
				bit30 ? 180 : 0, 
				(!bit29 && bit31) || (bit29 && !bit31) ? 180 : 0, 
				bit29 ? -90 : 0);
		}
	}
}

