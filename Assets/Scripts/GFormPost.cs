using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerFeedback {
	public static class GFormPost {

		public static bool isSending = false;

		private static GFormSettings settings;
		private static string url;

		public static GFormSettings Settings { get => settings; }

		public static void Initialise(GFormSettings settings) {
			GFormPost.settings = settings;
			if (settings.gFormURL.EndsWith("viewform")) {
				url = settings.gFormURL.Substring(0, settings.gFormURL.LastIndexOf("viewform")) + "formResponse";
			} else if (!settings.gFormURL.EndsWith("/")) {
				url = settings.gFormURL + "/formResponse";
			} else if (!settings.gFormURL.EndsWith("formResponse")) {
				url = settings.gFormURL + "formResponse";
			}
		}

		public static IEnumerator SendForm() {
			isSending = true;
			if (settings.logPlayerPos) {
				PositionLogger.CloseWriter();
			}
			WWWForm form = new WWWForm();

			//Fill in entries
			foreach (GFormSettings.gFormEntry entry in settings.gFormEntries) {
				if (entry.multipleSelection) {
					foreach (string str in entry.multipleValue) {
						form.AddField(entry.entryID, str);
					}
				} else {
					form.AddField(entry.entryID, entry.value);
				}
			}

			//Read playerPos
			if (settings.logPlayerPos) {
				StreamReader playerPosReader = new StreamReader(GFormSettings.playerPosPath, true);
				form.AddField(settings.playerPosEntryID, playerPosReader.ReadToEnd());
				playerPosReader.Close();
				PositionLogger.OpenWriter();
			}

			//Send the form
			UnityWebRequest www = UnityWebRequest.Post(url, form);
			Debug.Log("Sending form");
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError) {
				Debug.LogError("Connection error " + www.result + " " + www.error);
			} else if (www.result == UnityWebRequest.Result.DataProcessingError) {
				Debug.LogError("Data error " + www.result + " " + www.error);
			}
			Debug.Log("Form Sent " + www.result);
			www.Dispose();
			isSending = false;
		}
	}
}
