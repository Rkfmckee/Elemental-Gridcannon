using UnityEngine;

public class PlayerController : MonoBehaviour {
	#region Properties

	private PlayerState currentState;

	#endregion

	#region Events

	private void Start() {
		SetCurrentState(new PlayerStatePickupCard(gameObject));
	}

	private void Update() {
		currentState.Update();
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
