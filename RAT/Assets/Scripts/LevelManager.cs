using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Level;

public class LevelManager : MonoBehaviour {
	
	private string FIRST_LEVEL_NAME = "Part1.Laboratory1";//the very first level
	
	private static string nextLevelName;//used to keep the information between levels
	private static BaseNodeElement lastNodeElementTrigger;//last trigger, spawn, link => used to keep the information between levels

	private static string currentLevelName;
	private static NodeLevel currentNodeLevel;


	private static void loadNextLevel(string levelName) {
		
		nextLevelName = levelName;
		
		Application.LoadLevel(0);

	}

	void Start () {

		if(currentLevelName == null) {

			//load a level
			
			string levelName = null;
			
			//TODO load saved game, ex : levelName = loadSavedLevelName();
			
			if(levelName == null) {
				levelName = FIRST_LEVEL_NAME;
			}
			
			loadNextLevel(levelName);
		}


	}

	private static bool isAboutToLoadNextLevel() {
		return !(string.IsNullOrEmpty(nextLevelName));
	}

	void OnLevelWasLoaded(int level) {

		//loaded set the current name
		currentLevelName = nextLevelName;
		nextLevelName = null;
		
		Debug.Log("LOAD LEVEL : " + currentLevelName);

		createLevel();
		createMap();


		PlayerControls playerControls = FindObjectOfType<PlayerControls>();
		if(playerControls == null) {
			throw new System.InvalidOperationException();
		}


		BaseNodeElement currentNodeElementTrigger;

		if(lastNodeElementTrigger != null) {
			//something has triggered the load level (can be a link or a spawn)
			currentNodeElementTrigger = lastNodeElementTrigger;

		} else {

			//get spawn element of the level
			currentNodeElementTrigger = currentNodeLevel.spawnElement;
			if(currentNodeElementTrigger == null) {
				//replace by default spawn element
				currentNodeElementTrigger = new NodeElementSpawn();
			}
		}

		processPlayerLoad(playerControls, currentNodeElementTrigger);
		lastNodeElementTrigger = null;
	}

	private void createLevel() {
		
		TextAsset textAssetLevel = getLevelAsset(currentLevelName);
		if(textAssetLevel == null) {
			throw new System.InvalidOperationException("Could not load textAssetLevel : " + currentLevelName);
		}

		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAssetLevel.text);
		
		XmlElement rootNode = xmlDocument.DocumentElement;

		currentNodeLevel = new NodeLevel(rootNode.SelectSingleNode("node"));


		/*
		/// TODO DEBUG ///
		if(currentNodeLevel.spawnElement == null) {
			Debug.Log(">>> nodeLevel.spawnElement => null");
		} else {	
			Debug.Log(">>> nodeLevel.spawnElement => " + 
			          "x(" + currentNodeLevel.spawnElement.nodePosition.x + ") " +
			          "y(" + currentNodeLevel.spawnElement.nodePosition.y + ") " +
			          "direction(" + currentNodeLevel.spawnElement.nodeDirection.value + ")");
		}
		
		for(int i=0;i<currentNodeLevel.getHubCount();i++) {
			
			NodeElementHub hubElement = currentNodeLevel.getHub(i);
			
			Debug.Log(">>> nodeLevel.hubElement[" + i + "] => " + 
			          "x(" + hubElement.nodePosition.x + ") " +
			          "y(" + hubElement.nodePosition.y + ") " +
			          "direction(" + hubElement.nodeDirection.value + ")");
		}

		for(int i=0;i<currentNodeLevel.getLinkCount();i++) {

			NodeElementLink linkElement = currentNodeLevel.getLink(i);

			Debug.Log(">>> nodeLevel.linkElement[" + i + "] => " + 
			          "x(" + linkElement.nodePosition.x + ") " +
			          "y(" + linkElement.nodePosition.y + ") " +
			          "nextMap(" + linkElement.nodeNextMap.value + ") " +
			          "nextPos.x(" + linkElement.nodeNextPosition.x + ") " +
			          "nextPos.y(" + linkElement.nodeNextPosition.y + ") " +
					  "nextDirection(" + linkElement.nodeNextDirection.value + ")");
		}*/
		
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

	}


	private void createMap() {
		
		TextAsset textAssetMap = getMapAsset(currentLevelName);
		if(textAssetMap == null) {
			throw new System.InvalidOperationException("Could not load textAssetMap : " + currentLevelName);
		}

		Dictionary<string,object> dict = Json.Deserialize(textAssetMap.text) as Dictionary<string,object>;
		
		GameObject mapObject = this.gameObject;

		//generate map
		TiledMap.Map map = new TiledMap.Map(dict);
		map.instanciateMap(mapObject);


		// load links
		int linkCount = currentNodeLevel.getLinkCount();
		for(int i=0 ; i<linkCount ; i++) {

			NodeElementLink nodeElementLink = currentNodeLevel.getLink(i);
			GameObject prefabTile = UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_PREFABS + Constants.PREFAB_NAME_TILE_LINK, typeof(GameObject)) as GameObject;

			if(prefabTile == null) {
				throw new System.InvalidOperationException();
			}
			
			GameObject tileObject = GameObject.Instantiate(
				prefabTile, 
				new Vector2(nodeElementLink.nodePosition.x * Constants.TILE_SIZE, -nodeElementLink.nodePosition.y * Constants.TILE_SIZE), 
				Quaternion.identity) as GameObject;

			tileObject.transform.SetParent(mapObject.transform);

			tileObject.name = Constants.PREFAB_NAME_TILE_LINK;

			//display debug image
			if(Debug.isDebugBuild) {

				Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_DEBUG + "Link.png", typeof(Sprite)) as Sprite;
				
				SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();

				spriteRenderer.sprite = sprite;
				spriteRenderer.sortingLayerName = "objects";
			}

			Link link = tileObject.GetComponent<Link>();
			link.nodeElementLink = nodeElementLink;

		}

	}

	
	
	public static void processPlayerLoad(PlayerControls playerControls, BaseNodeElement nodeElement) {
		//move player in level with SPAWN / HUB / LINK / load from save

		if(nodeElement == null) {
			throw new System.InvalidOperationException();
		}

		NodePosition nodePosition = null;
		NodeDirection nodeDirection = null;

		if(nodeElement is NodeElementSpawn) {

			NodeElementSpawn nodeElementSpawn = nodeElement as NodeElementSpawn;
			
			nodePosition = nodeElementSpawn.nodePosition;
			nodeDirection = nodeElementSpawn.nodeDirection;

		} else if(nodeElement is NodeElementLink) {
			
			NodeElementLink nodeElementLink = nodeElement as NodeElementLink;
			
			nodePosition = nodeElementLink.nodeNextPosition;
			nodeDirection = nodeElementLink.nodeNextDirection;

		} else {

			throw new System.NotSupportedException("Not supported yet");
		}

		playerControls.setInitialPosition(nodePosition, nodeDirection);
	}

	public static void processPlayerDeath(PlayerControls playerControls) {

		//TODO
	}

	public static void processLink(PlayerControls playerControls, Collider2D linkCollider) {

		if(isAboutToLoadNextLevel()) {
			//already processing for next level
			return;
		}

		//move player in current level or load next level with LINK

		Link link = linkCollider.GetComponent<Link>();
		NodeElementLink nodeElementLink = link.nodeElementLink;

		string requiredLevelName = null;
		NodeString nodeNextMap = nodeElementLink.nodeNextMap;
		if(nodeNextMap != null) {
			requiredLevelName = nodeNextMap.value;
		}

		if(string.IsNullOrEmpty(requiredLevelName) || requiredLevelName.Equals(currentLevelName)) {

			//move player
			processPlayerLoad(playerControls, nodeElementLink);

		} else {

			//TODO save scene elements (player, dead enemies...)
			
			lastNodeElementTrigger = nodeElementLink;

			//load level
			loadNextLevel(requiredLevelName);
		}

	}



	public static TextAsset getLevelAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_MAPS + levelName + ".xml", typeof(TextAsset)) as TextAsset;
	}
	
	public static TextAsset getMapAsset(string levelName) {
		
		if(string.IsNullOrEmpty(levelName)) {
			return null;
		}
		return UnityEditor.AssetDatabase.LoadAssetAtPath(Constants.PATH_RES_MAPS + levelName + ".json", typeof(TextAsset)) as TextAsset;
	}

}
