public class NumberCard : Card {
	#region Methods

	public override void ActivateCard() {
		print("Picked up number card");

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

		var playerController = References.playerController;
		playerController.SetCurrentState(new PlayerStatePlaceCard());
	}

	#endregion
}
