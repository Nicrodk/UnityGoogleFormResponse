using System.Collections.Generic;
using UnityEngine;

namespace PlayerFeedback {
	[CreateAssetMenu(fileName = "GFormSetup", menuName = "ScriptableObjects/GFormSetup")]
	public class GFormSettings : ScriptableObject {
		public static string playerPosPath;
		public string gFormURL;

		public bool logPlayerPos;
		public string playerPosEntryID;
		public gFormEntry[] gFormEntries;

		[System.Serializable]
		public struct gFormEntry {
			public string name;
			public bool multipleSelection;
			public string entryID;
			[HideInInspector] public string value;
			[HideInInspector] public List<string> multipleValue;
		}

		public void AddSetValue(string name, string value) {
			int index = System.Array.FindIndex(gFormEntries, entry => entry.name == name);
			if (index == -1) {
				Debug.LogError("Could not find " + name + " within gForm entries");
				return;
			}

			if (gFormEntries[index].multipleSelection && !gFormEntries[index].multipleValue.Contains(value))
				gFormEntries[index].multipleValue.Add(value);
			else if (!gFormEntries[index].multipleSelection)
				gFormEntries[index].value = value;
		}

		public void ResetValues() {
			for (int i = 0; i < gFormEntries.Length; i++) {
				gFormEntries[i].value = null;
				gFormEntries[i].multipleValue.Clear();
			}
		}
	}
}
