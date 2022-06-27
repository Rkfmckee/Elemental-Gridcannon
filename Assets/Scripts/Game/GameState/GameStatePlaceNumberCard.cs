using System.Collections.Generic;
using UnityEngine;

public class GameStatePlaceNumberCard : GameStatePlaceCard
{
	#region Properties

	private List<CannonShot> cannonShots;

	#endregion

	#region Methods

	protected override void NextState()
	{
		if (cannonShots.Count == 0) {
			base.NextState();
			return;
		}

		var cannonPrefab   = Resources.Load<GameObject>("Prefabs/Cannon/Cannon");
		var cannonObject   = GameObject.Instantiate(cannonPrefab);
		var cannon         = cannonObject.GetComponent<Cannon>();
		var cannonPosition = cannonShots[0].cannonSlot.transform.position + cannon.GetSpawnHeightOffset();
		
		cannonObject.transform.position = cannonPosition;
		cannon.SetupShot(cannonShots);
	}

	protected override void EnableHighlight(GameObject target)
	{
		base.EnableHighlight(target);

		var numberSlot = target.GetComponent<NumberCardSlot>();
		cannonShots    = new List<CannonShot>();

		foreach (var cannonShot in numberSlot.GetCannonShots())
		{
			if (cannonShot.targetSlot.GetCards().Count == 0) continue;
			cannonShots.Add(cannonShot);

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
