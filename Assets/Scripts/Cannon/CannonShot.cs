public class CannonShot {
	#region Properties

	public NumberCardSlot cannonSlot;
	public NumberCardSlot[] ammunitionSlots;
	public EnemyCardSlot targetSlot;

	#endregion

	#region Constructors

	public CannonShot(NumberCardSlot cannon, NumberCardSlot[] ammunition, EnemyCardSlot target) {
		cannonSlot      = cannon;
		ammunitionSlots = ammunition;
		targetSlot      = target;
	}

	#endregion
}
