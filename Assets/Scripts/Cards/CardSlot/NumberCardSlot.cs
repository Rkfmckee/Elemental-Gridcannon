public class NumberCardSlot : CardSlot {
	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.number.Add(this);
	}

	#endregion

}
