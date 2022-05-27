using UnityEngine;

public class CameraController : MonoBehaviour {
	#region Events

	private void Awake() {
		References.camera = GetComponent<Camera>();
	}

	#endregion
}
