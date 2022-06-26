using System.Collections.Generic;
using UnityEngine;

public class GameStatePlaceNumberCard : GameStatePlaceCard
{
	#region Properties

	private List<CannonShot> cannonShots;

	#endregion

	#region Methods

	// protected override void NextState()
	// {

	// }

	protected override void EnableHighlight(GameObject target)
	{
		base.EnableHighlight(target);

		var numberSlot = target.GetComponent<NumberCardSlot>();
		cannonShots    = numberSlot.GetCannonShots();

		foreach (var cannonShot in cannonShots)
		{
			if (cannonShot.targetSlot.GetCards().Count == 0) continue;

			foreach (var ammunition in cannonShot.ammunitionSlots)
			{
				ammunition.HighlightAmmunition();
			}

			cannonShot.targetSlot.HighlightTarget();
		}
	}

	protected override void DisableHighlight(GameObject target)
	{
		base.DisableHighlight(target);

		var numberSlot = target.GetComponent<NumberCardSlot>();
		foreach (var cannonShot in numberSlot.GetCannonShots())
		{
			if (cannonShot.targetSlot.GetCards().Count == 0) continue;

			foreach (var ammunition in cannonShot.ammunitionSlots)
			{
				ammunition.UnhighlightAmmunition();
			}

			cannonShot.targetSlot.UnhighlightTarget();
		}
	}

	#endregion
}
