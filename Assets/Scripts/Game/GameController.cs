using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {
	#region Properties

	private GameState currentState;
	private BobDirection currentBobDirection;
	private Vector3 currentBobAmount;
	private Vector3 bobBottomPosition;
	private Vector3 bobTopPosition;
	private float bobTime;
	private float bobTimer;

	#endregion

	#region Events

	private void Awake() {
		References.gameController = this;

		currentBobDirection = BobDirection.Up;
		bobBottomPosition   = Vector3.zero;
		bobTopPosition      = new Vector3(0, 0.2f, 0);
		bobTime             = 1f;
		bobTimer            = 0f;
	}

	private void Start() {
		References.Cards.Slots.number = References.Cards.Slots.number.OrderBy(s => s.name).ToList();

		SetCurrentState(new GameStateStartGame());
	}

	private void Update() {
		if (currentState != null) currentState.Update();

		switch (currentBobDirection)
        {
            case BobDirection.Up:
                Bob(bobBottomPosition, bobTopPosition, BobDirection.Down);
                break;
            case BobDirection.Down:
                Bob(bobTopPosition, bobBottomPosition, BobDirection.Up);
                break;
        }
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

		public Vector3 GetCurrentBobAmount() {
			return currentBobAmount;
		}

		#endregion

	private void CleanupPreviousState() {
		if (currentState == null) {
			return;
		}

		currentState.CleanupState();
	}

	#region Methods

	private void Bob(Vector3 startPosition, Vector3 endPosition, BobDirection nextDirection)
    {
        currentBobAmount  = Vector3.Lerp(startPosition, endPosition, bobTimer / bobTime);
        bobTimer         += Time.deltaTime;

        if (Vector3.Distance(currentBobAmount, endPosition) == 0)  {
            currentBobDirection = nextDirection;
            bobTimer = 0;
        }
    }

	#endregion

	#endregion

	#region Enums

	public enum BobDirection
	{
		None,
		Up,
		Down
	}

	#endregion
}
