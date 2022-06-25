using UnityEngine;

public abstract class GameState {
	#region Properties

	protected int? highlightMask;
	protected GameObject lastTarget;

	protected GameObject gameObject;
	protected GameController gameController;
	protected Camera camera;

	#endregion
	
	#region Constructor

	public GameState() {
		gameController = References.gameController;
		camera         = References.camera;

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

	public virtual void CleanupState() {
		ClearLastHighlight();
	}

	protected abstract void LeftClicked(GameObject target);

	protected virtual bool ShouldEnableHighlight(GameObject target) {
		return true;
	}

	protected virtual void EnableHighlight(GameObject target) {
		target.GetComponent<Highlight>().EnableOutline(true);
	}

	protected virtual void DisableHighlight(GameObject target) {
		target.GetComponent<Highlight>().EnableOutline(false);
	}

	protected virtual void LookForHighlightable() {
		Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, highlightMask.Value)) {
			var currentTarget = hit.collider.gameObject;

			if (currentTarget == null) {
				Debug.Log($"{hit.collider.name} doesn't have a highlight script");
				return;
			}

			if ((currentTarget != lastTarget) && ShouldEnableHighlight(currentTarget)) {
				ClearLastHighlight();
				EnableHighlight(currentTarget);
				lastTarget = currentTarget;
			}

			if (Input.GetButtonDown("Fire1")) {
				LeftClicked(currentTarget.gameObject);
			}
			
		} else {
			ClearLastHighlight();
		}
	}

	protected void ClearLastHighlight() {
		if (lastTarget == null) {
			return;
		}

		DisableHighlight(lastTarget);
		lastTarget = null;
	}

	#endregion
}
