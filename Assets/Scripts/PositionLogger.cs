using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerFeedback {
	public class PositionLogger : MonoBehaviour {

		[SerializeField] private bool logOnEnable = false;
		[SerializeField] private GFormSettings settings;
		[SerializeField] private float delay = 0.1f;

		private static IEnumerator posLogger;
		private static StreamWriter writer;

		private void Awake() {
			if (GFormSettings.playerPosPath == null || GFormSettings.playerPosPath == "")
				GFormSettings.playerPosPath = Application.persistentDataPath + "/playerPos.txt";

			if (GFormPost.Settings == null && settings != null)
				GFormPost.Initialise(settings);
		}

		private void OnEnable() {
			if (logOnEnable) {
				OpenWriter();
				posLogger = LogPos();
				StartCoroutine(posLogger);
			}
		}

		private void OnDisable() {
			StopCoroutine(posLogger);
			CloseWriter();
		}

		public void StartLogging() {
			OpenWriter();
			posLogger = LogPos();
			StartCoroutine(posLogger);
		}

		public void StopLogging() {
			StopCoroutine(posLogger);
			CloseWriter();
		}

		private IEnumerator LogPos() {
			CultureInfo culture = new CultureInfo("en-GB");
			while (true) {
				yield return new WaitForSeconds(delay);
				if (writer != null) {
					string line = "\n[" + System.DateTime.UtcNow.ToString(culture) + "] : " + this.transform.position;
					try {
						writer.Write(line);
					} catch (System.Exception e) {
						Debug.LogWarning(e);
					}
				}
			}
		}

		public static async void OpenWriter() {
			await Task.Run(() => {
				while (GFormPost.isSending == true) {
					System.Threading.Thread.Sleep(100);
				}
			});
			writer = new StreamWriter(GFormSettings.playerPosPath, true);
		}

		public static void CloseWriter() {
			writer.Close();
			writer.Dispose();
		}
	}
}
