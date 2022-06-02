public class SpecialCardSlot : CardSlot {
	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.special.Add(this);
	}

	#endregion
}
