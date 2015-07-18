using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public abstract class GameElementSaver {

	public static readonly string FILE_NAME_EXTENSION_BACKUP = "_bak";
	public static readonly string FILE_NAME_EXTENSION_TMP = "_tmp";

	public abstract int getVersion();
	
	public abstract string getFileName();
	
	public abstract bool isLevelSpecific();
	
	public abstract GameElementSaver newPreviousGameElementSaver();//if the current is saver v2, give saver v1


	private string getBaseFilePath() {

		string levelDirectoryName = "";

		if(isLevelSpecific()) {
			levelDirectoryName = GameHelper.Instance.getLevelManager().getCurrentLevelName() + "/";
		}

		return Application.persistentDataPath + "/" + levelDirectoryName + getFileName();
	}

	public bool loadData() {

		string filePath = getBaseFilePath();

		if(File.Exists(filePath)) {
			return loadData(filePath);
		}

		string filePathBackup = filePath + FILE_NAME_EXTENSION_BACKUP;

		if(File.Exists(filePathBackup)) {

			if(loadData(filePathBackup)) {

				//if load complete copy the backup to the current file
				try {
					FileUtil.ReplaceFile(filePathBackup, filePath);
				} catch(Exception e) {
					Debug.LogException(e);
				}

				return true;
			}
		}

		return false;
	}
		
	protected bool loadData(string filePath) {

		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();

		if(player == null) {
			Debug.LogWarning("Player is null");
			return false;
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream f = File.Open(filePath, FileMode.Open);

		int version = 0;

		try {
			//unserialize version
			version = (int) bf.Deserialize(f);

			if(version == getVersion()) {

				//unserialize element
				unserializeElement(bf, f);
				
				return (f.Position == f.Length);
			}

		} catch(Exception e) {

			Debug.LogException(e);
			return false;

		} finally {
			f.Close();
		}

		//version is incorrect, try with previous unserializer
		GameElementSaver previousSaver = newPreviousGameElementSaver();
		while(previousSaver != null) {

			if(previousSaver.getVersion() == version) {
				//found the right one
				if(loadData()) {
					//save data with the current version
					saveData();
					return true;
				}
				return false;
			}

			previousSaver = previousSaver.newPreviousGameElementSaver();//get the previous of the previous
		}

		return false;
	}
	
	protected abstract void unserializeElement(BinaryFormatter bf, FileStream f);


	public bool saveData() {
		
		string filePath = getBaseFilePath();

		//save game in tmp file
		string filePathTmp = filePath + FILE_NAME_EXTENSION_TMP;
		if(!saveData(filePathTmp)) {
			
			//delete tmp file
			try {
				FileUtil.DeleteFileOrDirectory(filePathTmp);
			} catch(Exception e) {
				Debug.LogException(e);
			}
			return false;
		}

		string filePathBackup = filePath + FILE_NAME_EXTENSION_BACKUP;

		//if succeeded, replace backup file with current file, replace current file by tmp file
		bool hadFilePath = File.Exists(filePath);

		if(hadFilePath) {
			//if previous file exist, copy to the backup
			try {
				FileUtil.ReplaceFile(filePath, filePathBackup);
			} catch(Exception e) {
				Debug.LogException(e);
			}
		}

		//save
		try {
			FileUtil.ReplaceFile(filePathTmp, filePath);
		} catch(Exception e) {
			Debug.LogException(e);
		}

		if(!hadFilePath) {
			//copy new file to the backup
			try {
				FileUtil.ReplaceFile(filePath, filePathBackup);
			} catch(Exception e) {
				Debug.LogException(e);
			}
		}

		//delete tmp file
		try {
			FileUtil.DeleteFileOrDirectory(filePathTmp);
		} catch(Exception e) {
			Debug.LogException(e);
		}

		return true;

	}
	
	private bool saveData(string filePath) {

		Player player = GameHelper.Instance.getPlayerGameObject().GetComponent<Player>();
		
		if(player == null) {
			Debug.LogWarning("Player is null");
			return false;
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream f = File.Create(filePath);

		try {
			//serialize version
			bf.Serialize(f, getVersion());
			
			//serialize element
			serializeElement(bf, f);

			return true;

		} catch(Exception e) {
			
			Debug.LogException(e);
			return false;
			
		} finally {
			f.Close();
		}

	}

	protected abstract void serializeElement(BinaryFormatter bf, FileStream f);

}

