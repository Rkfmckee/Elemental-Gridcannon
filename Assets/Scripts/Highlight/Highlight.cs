using UnityEngine;

public class Highlight : MonoBehaviour {
	#region Properties

	private Outline outline;

	#endregion

	#region Events

	private void Awake() {
		outline = gameObject.AddComponent<Outline>();

		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineColor = Color.yellow;
		outline.OutlineWidth = 5f;
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
