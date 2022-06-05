using UnityEngine;

public class Highlight : MonoBehaviour {
	#region Properties

	private Outline outline;

	#endregion

	#region Events

	private void Awake() {
		outline = gameObject.AddComponent<Outline>();

		outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
		outline.OutlineColor = Color.yellow;
		outline.OutlineWidth = 10;
		outline.enabled = false;
	}

	#endregion

	#region Methods

	public bool IsOutlineActive() {
		return outline.enabled;
	}

	public void EnableOutline(bool enable) {
		outline.enabled = enable;
	}

	#endregion
}
