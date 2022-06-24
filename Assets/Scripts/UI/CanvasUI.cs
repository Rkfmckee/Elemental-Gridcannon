using UnityEngine;

public class CanvasUI : MonoBehaviour {
	#region Events

	private void Awake() {
		References.UI.canvas = this;
	}

	#endregion
}
