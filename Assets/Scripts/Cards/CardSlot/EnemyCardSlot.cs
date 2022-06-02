public class EnemyCardSlot : CardSlot {
	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.enemy.Add(this);
	}

	#endregion
}
