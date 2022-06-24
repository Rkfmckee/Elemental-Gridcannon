using UnityEngine;

public class GameStateStartGame : GameState {
	#region Constructor

	public GameStateStartGame() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardDeck");
		References.UI.gameState.SetText("Click deck to layout your gridcannon");
	}

	#endregion
	
	#region Methods

	protected override void LeftClicked(GameObject target) {
		var cardDeck = target.GetComponent<CardDeck>();
		cardDeck.LayoutInitialCards();
		References.UI.gameState.ClearText();
	}

	#endregion
}
