using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {
	#region Properties

	private GameState currentState;

	#endregion

	#region Events

	private void Awake() {
		References.gameController = this;
	}

	private void Start() {
		References.Cards.Slots.number = References.Cards.Slots.number.OrderBy(s => s.name).ToList();

		SetCurrentState(new GameStateStartGame());
	}

	private void Update() {
		if (currentState != null) currentState.Update();
	}


	#endregion

	#region Methods

		#region Get/Set

		public GameState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(GameState state) {
			CleanupPreviousState();
			currentState = state;

			if (state == null) {
				References.UI.gameState.ClearText();
			}
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
