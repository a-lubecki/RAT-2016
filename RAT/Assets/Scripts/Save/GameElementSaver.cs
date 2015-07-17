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


	public bool loadData() {

		string filePath = Application.persistentDataPath + "/" + getFileName();

		if(File.Exists(filePath)) {
			return loadData(filePath);
		}

		string filePathBackup = Application.persistentDataPath + "/" + getFileName() + FILE_NAME_EXTENSION_BACKUP;

		if(File.Exists(filePathBackup)) {

			if(loadData(filePathBackup)) {
				//if load complete copy the backup to the current file
				FileUtil.CopyFileOrDirectory(filePathBackup, filePath);
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

		try {
			//unserialize version
			int version = (int) bf.Deserialize(f);

			if(version != getVersion()) {
				return false;
			}

			//unserialize element
			unserializeElement(bf, f);
			
			return (f.Position == f.Length);

		} catch(Exception e) {

			Debug.LogException(e);
			return false;

		} finally {
			f.Close();
		}

	}
	
	protected abstract void unserializeElement(BinaryFormatter bf, FileStream f);


	public bool saveData() {
		
		//save game in tmp file
		string filePathTmp = Application.persistentDataPath + "/" + getFileName() + FILE_NAME_EXTENSION_TMP;
		if(!saveData(filePathTmp)) {
			
			//delete tmp file
			FileUtil.DeleteFileOrDirectory(filePathTmp);

			return false;
		}
		
		string filePath = Application.persistentDataPath + "/" + getFileName();
		string filePathBackup = Application.persistentDataPath + "/" + getFileName() + FILE_NAME_EXTENSION_BACKUP;

		//if succeeded, replace backup file with current file, replace current file by tmp file
		FileUtil.CopyFileOrDirectory(filePath, filePathBackup);
		FileUtil.CopyFileOrDirectory(filePathTmp, filePath);

		//delete tmp file
		FileUtil.DeleteFileOrDirectory(filePathTmp);
		
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

