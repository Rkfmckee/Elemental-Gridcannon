using UnityEngine;

public class LookForHighlight : MonoBehaviour {
	#region Properties

	private Highlight lastHighlight;
	private int highlightMask;

	private new Camera camera;

	#endregion
	
	#region Events

	private void Awake() {
		camera = GetComponent<Camera>();
		highlightMask =  1 << LayerMask.NameToLayer("Highlightable");
	}

	private void Update() {
		Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, highlightMask)) {
			var currentHighlight = hit.collider.GetComponent<Highlight>();

			if (currentHighlight == null) {
				print($"{hit.collider.name} doesn't have a highlight script");
				return;
			}

			if (!currentHighlight.IsOutlineActive()) {
				ClearLastHighlight();
				currentHighlight.EnableOutline(true);
				lastHighlight = currentHighlight;
			}
			
		} else {
			ClearLastHighlight();
		}
	}

	#endregion

	#region Methods

	private void ClearLastHighlight() {
		if (lastHighlight == null) {
			return;
		}

		lastHighlight.EnableOutline(false);
		lastHighlight = null;
	}

	#endregion
}
