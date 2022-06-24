using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class GameStatePlaceStartingEnemies : GameState {
	#region Properties

	private Card card;
	private TempEnemyCardSlot tempEnemyCardSlot;

	#endregion
	
	#region Constructor

	public GameStatePlaceStartingEnemies() : base() {
		highlightMask = 1 << LayerMask.NameToLayer("CardSlot");
		tempEnemyCardSlot = References.Cards.Slots.tempEnemy;

		card = tempEnemyCardSlot.PickupCard();
		
		References.UI.gameState.SetText("Place starting enemies");
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();

		if (card.GetCurrentState() == CardState.Placed) {
			if (tempEnemyCardSlot.GetCards().Count > 0) {
				card = tempEnemyCardSlot.PickupCard();
			} else {
				gameController.SetCurrentState(new GameStatePickupCard());
			}
		}
	}

	#endregion

	#region Methods

	public override void CleanupState() {
		base.CleanupState();

		foreach(var slot in References.Cards.Slots.active) {
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
		CleanupState();
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
