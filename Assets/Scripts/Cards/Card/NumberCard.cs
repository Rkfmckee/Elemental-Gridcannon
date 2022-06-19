public class NumberCard : Card {
	#region Methods

	public override void ActivateCard() {
		var cardValue = cardType.GetValue();
		var allSlots = References.Cards.Slots.number;
		foreach(var slot in allSlots) {
			if (slot.GetCards().Count == 0) {
				slot.SetCanPlaceCard(true);
				continue;
			}

			var slotValue = slot.GetTopCard().GetCardType().GetValue();

			if (cardValue >= slotValue) {
				slot.SetCanPlaceCard(true);
			}
		}

		References.gameController.SetCurrentState(new GameStatePlaceCard());
	}

	#endregion
}
