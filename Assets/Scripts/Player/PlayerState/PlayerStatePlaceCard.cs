using System.Collections.Generic;
using UnityEngine;
using static Card;

public class PlayerStatePlaceCard : PlayerState {
	#region Properties

	private Card card;

	private List<CardSlot> cardSlots;

	#endregion
	
	#region Constructor

	public PlayerStatePlaceCard() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardSlot");
		card = References.Cards.currentCard;
		cardSlots = References.Cards.Slots.active;
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
		playerController.SetCurrentState(new PlayerStatePickupCard());
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
