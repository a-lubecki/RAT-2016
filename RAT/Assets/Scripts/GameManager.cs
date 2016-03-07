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
	}

	public static GameManager Instance {

		get {
			if (instance == null) {
				instance = new GameManager();
				instance.loadNodeGame();
			}
			return instance;
		}
	}


	private NodeGame nodeGame;
	private Inventory inventory;

	private string currentLevelName;
	private NodeLevel currentNodeLevel;//optional


	public NodeGame getNodeGame() {
		return nodeGame;
	}

	public Inventory getInventory() {
		return inventory;
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


		//create player inventory
		createInventory();
	}

	private void createInventory() {

		inventory = new Inventory();

		// load items from save
		List<ItemInGridSaveData> itemsInGridSaveData = GameSaver.Instance.getItemsInGridSaveData();

		if(itemsInGridSaveData == null) {
			return;
		}

		foreach(ItemInGridSaveData itemData in itemsInGridSaveData) {
			ItemInGrid item = new ItemInGrid();
			itemData.assign(item);
			inventory.addItem(item);
		}

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


	public void saveGame(bool deleteNpcs) {
		
		GameSaver.Instance.savePlayer();
		GameSaver.Instance.savePlayerStats();
		GameSaver.Instance.saveHub();
		GameSaver.Instance.saveDoors();
		GameSaver.Instance.saveLoots();
		GameSaver.Instance.saveInventory();

		if(deleteNpcs) {
			GameSaver.Instance.deleteNpcs();
		} else {
			GameSaver.Instance.saveNpcs();
		}

		//commit
		GameSaver.Instance.saveAllToFile();

	}


}

