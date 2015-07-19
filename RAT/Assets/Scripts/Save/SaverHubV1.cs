using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaverHubV1 : GameElementSaver {

	public override int getVersion() {
		return 1;
	}

	public override string getFileName() {
		return "hub";
	}
	
	public override bool isLevelSpecific() {
		return true;
	}
	
	public override GameElementSaver newPreviousGameElementSaver() {
		return null;
	}


	private HubData unserializedHubData;

	protected override void unserializeElement(BinaryFormatter bf, FileStream f) {

		unserializedHubData = (HubData) bf.Deserialize(f);
	}

	protected override void assignUnserializedElement() {

		Hub hub = GameHelper.Instance.getHub();
		if(hub == null) {
			return;
		}

		unserializedHubData.assign(hub);
	}


	protected override bool serializeElement(BinaryFormatter bf, FileStream f) {
		
		Hub hub = GameHelper.Instance.getHub();
		if(hub == null) {
			return false;//no hub to serialize
		}

		HubData hubData = new HubData(hub);

		bf.Serialize(f, hubData);

		return true;
	}

}

[Serializable]
class HubData {

	private bool isActivated; 

	public HubData(Hub hub) {

		isActivated = hub.isActivated;
	}
	
	public void assign(Hub hub) {

		hub.init(isActivated);
	}

}

