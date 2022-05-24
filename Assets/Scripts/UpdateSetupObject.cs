using UnityEngine;

namespace PlayerFeedback {
	public class UpdateSetupObject : MonoBehaviour {
		[SerializeField] private GFormSettings settings;
		[SerializeField] private bool onAwake = false;

		private void Awake() {
			if (onAwake)
				GFormPost.Initialise(settings);
		}

		public void UpdateObject() {
			GFormPost.Initialise(settings);
		}
	}
}
