public class EnemyCardSlot : CardSlot {
	#region Properties

	private NumberCardSlot adjacentNumberSlot;

	#endregion
	
	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.enemy.Add(this);
	}

	#endregion

	#region Get/Set Methods

	public NumberCardSlot GetAdjacentNumberSlot() {
		return adjacentNumberSlot;
	}

	public void SetAdjacentNumberSlot(NumberCardSlot slot) {
		adjacentNumberSlot = slot;
	}

	#endregion
}
