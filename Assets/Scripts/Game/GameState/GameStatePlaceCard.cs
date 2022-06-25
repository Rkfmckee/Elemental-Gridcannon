using System.Collections.Generic;
using UnityEngine;
using static Card;

public class GameStatePlaceCard : GameState {
	#region Properties

	private Card card;

	private List<CardSlot> cardSlots;

	#endregion
	
	#region Constructor

	public GameStatePlaceCard() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardSlot");
		card          = References.Cards.currentCard;
		cardSlots     = References.Cards.Slots.active;

		References.UI.gameState.SetText("Click a slot to place the card");
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();
	}

	#endregion

	#region Methods

	public override void CleanupState() {
		base.CleanupState();

		foreach(var slot in cardSlots) {
			slot.SetCanPlaceCard(false);
		}

		References.Cards.Slots.active.Clear();
	}

	protected override void LeftClicked(GameObject target) {
		var cardSlot = target.GetComponent<CardSlot>();
		if (!cardSlot.CanPlaceCard()) {
			return;
		}

		cardSlot.AddCard(card);
		References.Cards.currentCard = null;
		gameController.SetCurrentState(new GameStatePickupCard());
	}

	protected override bool ShouldEnableHighlight(GameObject target)
	{
		var cardSlot = target.GetComponent<CardSlot>();
		return cardSlot.CanPlaceCard();
	}

	protected override void EnableHighlight(GameObject target) {
		target.GetComponent<CardSlot>().HighlightDefault();
	}

	protected override void DisableHighlight(GameObject target) {
		target.GetComponent<CardSlot>().HighlightTranslucent();
	}

	protected override void LookForHighlightable() {
		if (card.GetCurrentState() == CardState.Moving) {
			return;
		}

		base.LookForHighlightable();
	}

	#endregion
}
