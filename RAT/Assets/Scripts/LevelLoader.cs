using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Level;

public class LevelLoader : MonoBehaviour {

	public static TextAsset textAssetLevel;

	private TextAsset textAssetMap;//TODO set to private
	
	void OnLevelWasLoaded(int level) {

		//TODO
	}

	void Start () {
		loadLevel();
		loadMap();
	}
	

	private void loadLevel() {
		
		if(textAssetLevel == null) {
			//load the first level
			textAssetLevel = Resources.LoadAssetAtPath(Constants.PATH_RES_TEST + "level1.xml", typeof(TextAsset)) as TextAsset;
		}
		
		//TODO load textAssetLevel then assign textAssetMap 
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAssetLevel.text);
		
		XmlElement rootNode = xmlDocument.DocumentElement;

		XmlNode mainNode = rootNode.SelectSingleNode("node");


		////TODO TEST
		foreach (XmlNode node in mainNode) {
			
			if("pos".Equals(LevelNode.getNodeText(node))) {
				
				Position p = new Position(node);
				Debug.Log(">>> DONE !!! => pos : " + p.x + " / " + p.y);
				
				break;
			}
		}


		//TODO test, assign the textAssetMap from the level
		textAssetMap = Resources.LoadAssetAtPath(Constants.PATH_RES_TEST + "test_map2.json", typeof(TextAsset)) as TextAsset;
	
	}


	private void loadMap() {
		
		if(textAssetMap == null) {
			throw new System.InvalidOperationException();
		}

		Dictionary<string,object> dict = Json.Deserialize(textAssetMap.text) as Dictionary<string,object>;
		
		//generate map
		TiledMap.Map map = new TiledMap.Map(dict);
		
		map.instanciateMap(this.gameObject);
	}

}
