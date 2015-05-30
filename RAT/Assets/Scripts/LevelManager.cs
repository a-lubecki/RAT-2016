using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Level;

public class LevelManager : MonoBehaviour {

	private string FIRST_LEVEL = Constants.PATH_RES_TEST + "level1.xml";

	public static TextAsset textAssetLevel;

	private TextAsset textAssetMap;//TODO set to private

	public static NodeLevel level { get; private set; }

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
			textAssetLevel = Resources.LoadAssetAtPath(FIRST_LEVEL, typeof(TextAsset)) as TextAsset;
		}
		
		//TODO load textAssetLevel then assign textAssetMap 
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAssetLevel.text);
		
		XmlElement rootNode = xmlDocument.DocumentElement;

		level = new NodeLevel(rootNode.SelectSingleNode("node"));

		/// TODO DEBUG ///
		if(level.spawnElement == null) {
			Debug.Log(">>> nodeLevel.spawnElement => null");
		} else {	
			Debug.Log(">>> nodeLevel.spawnElement => " + 
			          "x(" + level.spawnElement.nodePosition.x + ") " +
			          "y(" + level.spawnElement.nodePosition.y + ") " +
			          "direction(" + level.spawnElement.nodeDirection.value + ")");
		}
		
		for(int i=0;i<level.getHubCount();i++) {
			
			NodeElementHub hubElement = level.getHub(i);
			
			Debug.Log(">>> nodeLevel.hubElement[" + i + "] => " + 
			          "x(" + hubElement.nodePosition.x + ") " +
			          "y(" + hubElement.nodePosition.y + ") " +
			          "direction(" + hubElement.nodeDirection.value + ")");
		}

		for(int i=0;i<level.getLinkCount();i++) {

			NodeElementLink linkElement = level.getLink(i);

			Debug.Log(">>> nodeLevel.linkElement[" + i + "] => " + 
			          "x(" + linkElement.nodePosition.x + ") " +
			          "y(" + linkElement.nodePosition.y + ") " +
			          "nextMap(" + linkElement.nodeNextMap.value + ") " +
			          "nextPos.x(" + linkElement.nodeNextPosition.x + ") " +
			          "nextPos.y(" + linkElement.nodeNextPosition.y + ") " +
					  "nextDirection(" + linkElement.nodeNextDirection.value + ")");
		}
		
		/*   
		   ////TODO TEST
		   foreach (XmlNode node in mainNode) {
			
			if("pos".Equals(LevelNode.getText(node))) {
				
				NodePosition n = new NodePosition(node);
				Debug.Log(">>> DONE !!! => pos : " + n.x + " / " + n.y);
				
			} else if("level".Equals(LevelNode.getText(node))) {
				
				LevelNodeInt n = new LevelNodeInt(node);
				Debug.Log(">>> DONE !!! => level : " + n.value);
				
			} else if("name".Equals(LevelNode.getText(node))) {
				
				LevelNodeString n = new LevelNodeString(node);
				Debug.Log(">>> DONE !!! => name : " + n.value);
				
			} else if("label".Equals(LevelNode.getText(node))) {
				
				LevelNodeLabel n = new LevelNodeLabel(node);
				Debug.Log(">>> DONE !!! => label : " + n.value);
			}*/
		/// TODO DEBUG ///


		//TODO test, assign the textAssetMap from the level
		textAssetMap = Resources.LoadAssetAtPath(Constants.PATH_RES_TEST + "test_map2.json", typeof(TextAsset)) as TextAsset;
	
	}


	private void loadMap() {
		
		if(textAssetMap == null) {
			throw new System.InvalidOperationException();
		}

		Dictionary<string,object> dict = Json.Deserialize(textAssetMap.text) as Dictionary<string,object>;
		
		GameObject mapObject = this.gameObject;

		//generate map
		TiledMap.Map map = new TiledMap.Map(dict);
		map.instanciateMap(mapObject);


		// load links
		int linkCount = level.getLinkCount();
		for(int i=0 ; i<linkCount ; i++) {

			NodeElementLink link = level.getLink(i);
			GameObject prefabTile = Resources.LoadAssetAtPath(Constants.PATH_PREFABS + Constants.PREFAB_NAME_TILE_LINK, typeof(GameObject)) as GameObject;

			if(prefabTile == null) {
				throw new System.InvalidOperationException();
			}
			
			GameObject tileObject = GameObject.Instantiate(
				prefabTile, 
				new Vector2(link.nodePosition.x * Constants.TILE_SIZE, -link.nodePosition.y * Constants.TILE_SIZE), 
				Quaternion.identity) as GameObject;

			tileObject.transform.SetParent(mapObject.transform);

			tileObject.name = Constants.PREFAB_NAME_TILE_LINK;
		}

	}

	public static void loadNextMap(PlayerControls playerControls, Collider2D linkCollider) {
		//TODO
		Debug.Log("loadNextMap !!!");
	}

}
