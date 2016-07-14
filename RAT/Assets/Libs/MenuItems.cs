using UnityEngine;
using UnityEditor;

// https://unity3d.com/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items?playlist=17117

public class MenuItems {
	
	[MenuItem("R.A.T./Delete game save")]
	private static void deleteSaveGame() {
		
		GameSaver.Instance.deleteSave();

	}

}