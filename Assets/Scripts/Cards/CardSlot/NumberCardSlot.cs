using System.Collections.Generic;
using UnityEngine;

public class NumberCardSlot : CardSlot {
	#region Properties

	[SerializeField]
	private List<EnemyCardSlot> adjacentEnemySlots;

	#endregion
	
	#region Methods

		#region Get/Set

		public List<EnemyCardSlot> GetAdjacentEnemySlots() {
			return adjacentEnemySlots;
		}

		#endregion

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.number.Add(this);
	}

	#endregion

}
