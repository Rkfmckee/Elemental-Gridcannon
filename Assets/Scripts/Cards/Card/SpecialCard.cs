public class SpecialCard : Card {
	#region Methods

	public override void ActivateCard() {
		var specialSlot = References.Cards.Slots.special;
		specialSlot.AddCard(this);

		References.gameController.SetCurrentState(new GameStatePickupCard());
	}

	#endregion
}
