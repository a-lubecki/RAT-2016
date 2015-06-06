using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMap {

	public class Map {

		public int width { get; private set; }
		public int height { get; private set; }
		
		public int tileWidth { get; private set; }
		public int tileHeight { get; private set; }

		private TileDescriptorSet[] tileDescriptorSet;
		private Layer[] layers;

		private Dictionary<int, TileDescriptor> tileDescriptors = new Dictionary<int, TileDescriptor>();

		public Map(Dictionary<string, object> dict) {

			//UnityEngine.Debug.Log("bbb");

			width = (int)(long)dict["width"];
			height = (int)(long)dict["height"];
			tileWidth = (int)(long)dict["tilewidth"];
			tileHeight = (int)(long)dict["tileheight"];

			List<object> tileDescriptorSetJson = (List<object>)dict["tilesets"];
			tileDescriptorSet = new TileDescriptorSet[tileDescriptorSetJson.Count];
			int i = 0;
			foreach (Dictionary<string, object> d in tileDescriptorSetJson) {
				tileDescriptorSet[i++] = new TileDescriptorSet(this, d);
			}

			List<object> layersJson = (List<object>)dict["layers"];
			layers = new Layer[layersJson.Count];
			i = 0;
			foreach (Dictionary<string, object> d in layersJson) {
				layers[i++] = new Layer(this, d);
			}
		}

		public int getNbTileDescriptorSets() {
			return tileDescriptorSet.Length;
		}

		public TileDescriptorSet getTileDescriptorSetAt(int index) {
			if(index < 0) {
				throw new System.ArgumentException();
			}			
			if(index >= getNbTileDescriptorSets()) {
				throw new System.ArgumentException();
			}
			return tileDescriptorSet[index];
		}
		
		public int getNbLayers() {
			return layers.Length;
		}
		
		public Layer getLayerAt(int index) {
			if(index < 0) {
				throw new System.ArgumentException();
			}			
			if(index >= getNbTileDescriptorSets()) {
				throw new System.ArgumentException();
			}
			return layers[index];
		}


		public void registerTileDescriptor(TileDescriptor tileDescriptor) {
			tileDescriptors.Add(tileDescriptor.id, tileDescriptor);
		}

		public TileDescriptor getTileDescriptor(int id) { 

			if(!tileDescriptors.ContainsKey(id)) {
				return null;
			}

			return tileDescriptors[id];
		}


		public void instanciateMap(GameObject mapObject) {
			
			if(mapObject == null) {
				throw new System.InvalidOperationException();
			}

			foreach (Layer layer in layers) {

				int w = layer.nbElemsX;
				int h = layer.nbElemsY;
				
				string layerName = layer.sortingLayerName;
				int orderInLayer = layer.orderInLayer;
				
				//TODO layer name

				for (int y = 0 ; y < h ; y++) {
					for (int x = 0 ; x < w ; x++) {
					
						Tile tile = layer.getTileAt(x, y);

						if(tile == null) {
							//no tile here
							continue;
						}

						TileDescriptor tileDescriptor = tile.tileDescriptor;
						if(tileDescriptor == null) {
							throw new System.InvalidOperationException();
						}

						Sprite sprite = tileDescriptor.tileSprite;

						GameObject prefabTile;

						bool isWall = layerName.Equals("walls");

						string tileName;
						if(isWall) {
							tileName = Constants.PREFAB_NAME_TILE_WALL;
						} else {
							tileName = Constants.PREFAB_NAME_TILE_GROUND;
						}

						prefabTile = Resources.LoadAssetAtPath(Constants.PATH_PREFABS + tileName, typeof(GameObject)) as GameObject;
						if(prefabTile == null) {
							throw new System.InvalidOperationException();
						}

						GameObject tileObject = GameObject.Instantiate(prefabTile, new Vector2(x * Constants.TILE_SIZE, -y * Constants.TILE_SIZE), Quaternion.identity) as GameObject;
						tileObject.transform.SetParent(mapObject.transform);

						tileObject.name = tileName;

						//walls are not displayed
						if(!isWall) {
							SpriteRenderer spriteRenderer = tileObject.GetComponent<SpriteRenderer>();
							
							spriteRenderer.sprite = sprite;
							spriteRenderer.sortingLayerName = layerName;
							spriteRenderer.sortingOrder = orderInLayer;
						}
					}
				}

			}

		}

	}
}
