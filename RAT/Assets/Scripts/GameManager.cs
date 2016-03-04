using UnityEngine;
using UnityEngine.SceneManagement;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Node;
using System;

public class GameManager {
	
	private static GameManager instance;

	private GameManager() {

		loadNodeGame();
	}

	public static GameManager Instance {

		get {
			if (instance == null) {
				instance = new GameManager();
			}
			return instance;
		}
	}

	private NodeGame nodeGame;

	private string currentLevelName;
	private NodeLevel currentNodeLevel;//optional


	public NodeGame getNodeGame() {
		return nodeGame;
	}

	public bool hasCurrentLevel() {
		return !string.IsNullOrEmpty(currentLevelName);
	}

	public string getCurrentLevelName() {
		return currentLevelName;
	}

	public NodeLevel getCurrentNodeLevel() {
		return currentNodeLevel;
	}


	public void loadNodeGame() {

		if(nodeGame != null) {
			//node already loaded
			return;
		}

		TextAsset textAssetItemsPatterns = GameHelper.Instance.loadTextAsset(Constants.PATH_RES_ITEMS + "Item.Patterns");
		if(textAssetItemsPatterns == null) {
			Debug.LogWarning("Could not load textAssetItemsPatterns");
			return;
		}

		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAssetItemsPatterns.text);

		XmlElement rootNode = xmlDocument.DocumentElement;

		nodeGame = new NodeGame(rootNode.SelectSingleNode("node"));

	}


	public void loadNodeLevel(string nextLevelName) {
	
		if(string.IsNullOrEmpty(nextLevelName)) {
			throw new ArgumentException();
		}

		if(nextLevelName.Equals(currentLevelName)) {
			//already loaded
			return;
		}

		TextAsset textAssetLevel = GameHelper.Instance.loadLevelAsset(nextLevelName);
		if(textAssetLevel == null) {
			Debug.LogWarning("Could not load textAssetLevel : " + nextLevelName);
			return;
		}

		currentLevelName = nextLevelName;


		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAssetLevel.text);

		XmlElement rootNode = xmlDocument.DocumentElement;

		currentNodeLevel = new NodeLevel(rootNode.SelectSingleNode("node"));

	}


}

