using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Security.Cryptography;
using System.Text;

public abstract class GameElementSaver {

	public static readonly string FILE_NAME_EXTENSION_BACKUP = "_bak";
	public static readonly string FILE_NAME_EXTENSION_TMP = "_tmp";

	public abstract int getVersion();
	
	public abstract string getFileName();
	
	public abstract bool isLevelSpecific();
	
	public abstract GameElementSaver newPreviousGameElementSaver();//if the current is saver v2, give saver v1


	private string getBaseFilePath() {

		if(!isLevelSpecific()) {
			return Application.persistentDataPath + "/" + getFileName();
		}

		string path = Application.persistentDataPath + "/" + GameHelper.Instance.getLevelManager().getCurrentLevelName();

		if(!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}

		return path + "/" + getFileName();
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

				//check the last editing and the checksum
				long oldEditingDateTime = (long) bf.Deserialize(f);
				long currentEditingDateTime = getFileLastWriteTime(filePath);
				if(currentEditingDateTime != oldEditingDateTime) {
					Debug.LogError("Unserialize problem, date difference, file was modified outside of the game : " +
					               currentEditingDateTime + " / " + oldEditingDateTime + ")");
					return false;
				}

				int currentDataPosition = (int)f.Position;
				string oldChecksum = (string) bf.Deserialize(f);
				string currentChecksum = getCheckSum(f, currentDataPosition);
				if(!currentChecksum.Equals(oldChecksum)) {
					Debug.LogError("Unserialize problem, checksum difference, file was modified outside of the game : " + currentChecksum + " / " + oldChecksum);
					return false;
				}

				//check if the unserializing is correct
				if(f.Position != f.Length) {
					Debug.LogError("Unserialize problem, end of file not reached : " + f.Position + " / " + f.Length);
					return false;
				}

				assignUnserializedElement();

				return true;
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

	protected abstract void assignUnserializedElement();

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

		long editingDateTime = 0;
		try {
			//serialize version
			bf.Serialize(f, getVersion());
			
			//serialize element
			if(!serializeElement(bf, f)) {
				//the serialization has been stopped by the subclass
				return false;
			}

			//save edition date + hash to avoid editing the files
			editingDateTime = getFileLastWriteTime(filePath);
			bf.Serialize(f, editingDateTime);
			bf.Serialize(f, getCheckSum(f, (int)f.Position));

		} catch(Exception e) {
			
			Debug.LogException(e);
			return false;
			
		} finally {
			f.Close();
		}

		long currentEditingDateTime = getFileLastWriteTime(filePath);
		if(editingDateTime != currentEditingDateTime) {
			Debug.LogError("Serialize problem, date difference : " +
			               currentEditingDateTime + " / " + editingDateTime + ")");
			return false;
		}

		return true;
	}

	protected abstract bool serializeElement(BinaryFormatter bf, FileStream f);


	private static long getFileLastWriteTime(string filePath) {

		DateTime dateTime = File.GetLastWriteTime(filePath);

		//truncate to avoid save problems
		return (long)Math.Floor((double)dateTime.Ticks / 10000000d);
	}

	public static DateTime truncateDateTime(DateTime dateTime, TimeSpan timeSpan) {
		if (timeSpan == TimeSpan.Zero) {
			return dateTime;
		}
		return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
	}


	private static string getCheckSum(FileStream f, int length) {

		BinaryReader br = new BinaryReader(f);
		byte[] bytes = br.ReadBytes(length);

		SHA1 sha1 = SHA1.Create();
		byte[] hashBytes = sha1.ComputeHash(bytes);

		StringBuilder checksum = new StringBuilder();
		//convert hash bytes to string
		foreach (byte b in hashBytes) {
			string hex = b.ToString("x2");
			checksum.Append(hex);
		}

		return checksum.ToString();
	}

}

