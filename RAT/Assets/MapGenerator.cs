using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
	
	//public string mapFilePath;
	public TextAsset textAsset;

	// Use this for initialization
	void Start () {
	
		if(textAsset == null) {
			throw new System.InvalidOperationException();
		}
		
		Dictionary<string,object> dict = Json.Deserialize(textAsset.text) as Dictionary<string,object>;

		//generate map
		TiledMap.Map map = new TiledMap.Map(dict);

		map.instanciateMap(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
