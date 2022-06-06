public class SpecialCard : Card {
	#region Methods

	public override void ActivateCard() {
		var specialSlot = References.Cards.Slots.special;
		specialSlot.AddCard(this);

		References.playerController.SetCurrentState(new PlayerStatePickupCard());
	}

	#endregion
}
