using UnityEngine;

public class GameStateStartGame : GameState {
	#region Constructor

	public GameStateStartGame() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardDeck");
	}

	#endregion
	
	#region Methods

	protected override void LeftClicked(GameObject target) {
		var cardDeck = target.GetComponent<CardDeck>();
		cardDeck.LayoutInitialCards();
	}

	#endregion
}
