using UnityEngine;
using UnityEngine.UI;
using PlayerFeedback;

public class SendForm : MonoBehaviour {
	[SerializeField] private string entryName;
	[SerializeField] private InputField textField;

	public void SendAnswers() {
		GFormPost.Settings.AddSetValue(entryName, textField.text);
		textField.text = null;
		StartCoroutine(GFormPost.SendForm());
	}
}
