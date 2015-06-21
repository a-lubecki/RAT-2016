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

		[Obsolete("Remove this and update 'ground' tiles around player with GameObjects pooling : http://blogs.msdn.com/b/dave_crooks_dev_blog/archive/2014/07/21/object-pooling-for-unity3d.aspx",false)]
		public void instanciateMap(GameObject mapObject) {
			
			if(mapObject == null) {
				throw new System.InvalidOperationException();
			}

			foreach (Layer layer in layers) {

				int w = layer.nbElemsX;
				int h = layer.nbElemsY;
				
				string layerName = layer.sortingLayerName;
				int orderInLayer = layer.orderInLayer;
				
				GameObject prefabTileGround = GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_GROUND);
				if(prefabTileGround == null) {
					throw new System.InvalidOperationException();
				}
				GameObject prefabTileWall = GameHelper.Instance.loadPrefabAsset(Constants.PREFAB_NAME_TILE_WALL);
				if(prefabTileWall == null) {
					throw new System.InvalidOperationException();
				}

				
				WallCreator wallCreator = new WallCreator();
				GroundCreator groundCreator = new GroundCreator();

				for (int y = 0 ; y < h ; y++) {
					for (int x = 0 ; x < w ; x++) {
					
						Tile tile = layer.getTileAt(x, y);

						if(tile == null) {
							//no tile here
							continue;
						}

						bool isWall = layerName.Equals(Constants.SORTING_LAYER_NAME_WALLS);
						if(isWall) {
							wallCreator.createNewGameObject(x, y, tile);
						} else {
							groundCreator.createNewGameObject(x, y, tile, orderInLayer);
						}

					}
				}

			}

		}

	}
}
