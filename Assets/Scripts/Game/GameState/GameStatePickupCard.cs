using UnityEngine;

public class GameStatePickupCard : GameState {
	#region Constructor

	public GameStatePickupCard() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardDeck");
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
			gameController.SetCurrentState(null);
		}
	}

	#endregion
}
