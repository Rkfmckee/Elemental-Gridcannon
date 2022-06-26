using UnityEngine;

public class GameStatePickupCard : GameState {
	#region Constructor

	public GameStatePickupCard() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardDeck");

		References.UI.gameState.SetText("Click deck to pick-up a card");
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();
	}

	#endregion

	#region Methods

	protected override void LeftClicked(GameObject target) {
		var cardDeck = target.GetComponent<CardDeck>();

		if (cardDeck.PickupCard()) {
			NextState();
		}
	}

	protected override void NextState()
	{
		gameController.SetCurrentState(null);
	}

	#endregion
}
