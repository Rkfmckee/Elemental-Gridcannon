using UnityEngine;

public class PlayerController : MonoBehaviour {
	#region Properties

	private PlayerState currentState;

	#endregion

	#region Events

	private void Awake() {
		References.playerController = this;
	}

	private void Start() {
		SetCurrentState(new PlayerStatePickupCard());
	}

	private void Update() {
		if (currentState != null) currentState.Update();
	}


	#endregion

	#region Methods

		#region Get/Set

		public PlayerState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(PlayerState state) {
			CleanupPreviousState();
			currentState = state;
		}

		#endregion

	private void CleanupPreviousState() {
		if (currentState == null) {
			return;
		}

		currentState.CleanupState();
	}

	#endregion
}
