using System.Collections.Generic;
using UnityEngine;
using static Card;

public class PlayerStatePlaceCard : PlayerState {
	#region Properties

	private Card card;

	private List<CardSlot> cardSlots;

	#endregion
	
	#region Constructor

	public PlayerStatePlaceCard(GameObject gameObj) : base(gameObj) {
		highlightMask = 1 << LayerMask.NameToLayer("CardSlot");
		card = References.Cards.currentCard;

		CheckWhichCardSlots();
		foreach(var slot in cardSlots) {
			slot.SetCanPlaceCard(true);
		}
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
	}

	protected override void LeftClicked(GameObject target) {
		var cardSlot = target.GetComponent<CardSlot>();
		if (!cardSlot.CanPlaceCard()) {
			return;
		}

		cardSlot.AddCard(card);
		
		playerController.SetCurrentState(new PlayerStatePickupCard(gameObject));
		References.Cards.currentCard = null;
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

	private void CheckWhichCardSlots() {
		if (card is NumberCard) {
			cardSlots = References.Cards.Slots.number;
			return;
		}

		if (card is EnemyCard) {
			cardSlots = References.Cards.Slots.enemy;
			return;
		}

		if (card is SpecialCard) {
			cardSlots = References.Cards.Slots.special;
			return;
		}
	}

	#endregion
}
