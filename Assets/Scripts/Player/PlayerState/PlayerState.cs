using UnityEngine;

public abstract class PlayerState {
	#region Properties

	protected int? highlightMask;
	protected Highlight lastHighlight;

	protected GameObject gameObject;
	protected PlayerController playerController;
	protected Camera camera;

	#endregion
	
	#region Constructor

	public PlayerState(GameObject gameObj) {
		gameObject = gameObj;
		playerController = gameObject.GetComponent<PlayerController>();
		camera = References.camera;

		highlightMask = null;
	}

	#endregion

	#region Events

	public virtual void Update() {
		if (highlightMask.HasValue) {
			LookForHighlightable();
		}
	}

	#endregion

	#region Methods

	protected abstract void LeftClicked(GameObject target);

	private void LookForHighlightable() {
		Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, highlightMask.Value)) {
			var currentHighlight = hit.collider.GetComponent<Highlight>();

			if (currentHighlight == null) {
				Debug.Log($"{hit.collider.name} doesn't have a highlight script");
				return;
			}

			if (!currentHighlight.IsOutlineActive()) {
				ClearLastHighlight();
				currentHighlight.EnableOutline(true);
				lastHighlight = currentHighlight;
			}

			if (Input.GetButtonDown("Fire1")) {
				LeftClicked(currentHighlight.gameObject);
			}
			
		} else {
			ClearLastHighlight();
		}
	}

	public void ClearLastHighlight() {
		if (lastHighlight == null) {
			return;
		}

		lastHighlight.EnableOutline(false);
		lastHighlight = null;
	}

	#endregion
}
