public class CannonShot {
	#region Properties

	public NumberCardSlot[] ammunitionSlots;
	public EnemyCardSlot targetSlot;

	#endregion

	#region Constructors

	public CannonShot(NumberCardSlot[] ammunition, EnemyCardSlot target) {
		ammunitionSlots = ammunition;
		targetSlot = target;
	}

	#endregion
}
