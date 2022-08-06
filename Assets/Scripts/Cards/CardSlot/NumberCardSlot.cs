using System;
using System.Collections.Generic;
using UnityEngine;

public partial class NumberCardSlot : CardSlot
{
	#region Properties

	[SerializeField]
	private List<EnemyCardSlot> adjacentEnemySlots;
	private List<CannonShot> cannonShots;
	private Color ammunitionColour;

	#endregion

	#region Events

	protected override void Awake()
	{
		base.Awake();

		References.Cards.Slots.number.Add(this);

		foreach (var slot in adjacentEnemySlots)
		{
			slot.SetAdjacentNumberSlot(this);
		}

		var currentCannonId = Int32.Parse(gameObject.name.Split("_")[1]);
		cannonShots = new List<CannonShot>();

		for (int i = 0; i < ammunitionAndTargetIds.GetLength(0); i++)
		{
			var cannonId = ammunitionAndTargetIds[i, 0];
			if (currentCannonId != cannonId) continue;

			var cannon     = GameObject.Find($"NumberCardSlot_{cannonId}").GetComponent<NumberCardSlot>();
			var target     = GameObject.Find($"EnemyCardSlot_{ammunitionAndTargetIds[i, 3]}").GetComponent<EnemyCardSlot>();
			var ammunition = new NumberCardSlot[] {
				GameObject.Find($"NumberCardSlot_{ammunitionAndTargetIds[i, 1]}").GetComponent<NumberCardSlot>(),
				GameObject.Find($"NumberCardSlot_{ammunitionAndTargetIds[i, 2]}").GetComponent<NumberCardSlot>()
			};

			cannonShots.Add(new CannonShot(cannon, ammunition, target));
		}

		ammunitionColour = Color.green;
	}

	#endregion

	#region Methods

		#region Get/Set

		public List<EnemyCardSlot> GetAdjacentEnemySlots()
		{
			return adjacentEnemySlots;
		}

		public List<CannonShot> GetCannonShots() {
			return cannonShots;
		}

		#endregion

	public void HighlightAmmunition() {
		ShowCardSlot(true);

		foreach(var particleSystem in particleSystems) {
			particleSystem.material.SetColor("_TintColor", ammunitionColour);
		}
	}

	public void UnhighlightAmmunition() {
		var showCard = References.Cards.Slots.active.Contains(this);
		ShowCardSlot(showCard);

		HighlightTranslucent();
	}

	#endregion

	#region Helper properties

	private int[,] ammunitionAndTargetIds
	{
		// cannon, ammunition1, ammunition2, target
		get
		{
			return new int[12, 4] {
				{ 1, 2, 3, 4  },
				{ 1, 4, 7, 9  },
				{ 2, 5, 8, 8  },
				{ 3, 2, 1, 12 },
				{ 3, 6, 9, 7  },
				{ 4, 5, 6, 5  },
				{ 6, 5, 4, 11 },
				{ 7, 8, 9, 6  },
				{ 7, 4, 1, 1  },
				{ 8, 5, 2, 2  },
				{ 9, 8, 7, 10 },
				{ 9, 6, 3, 3  }
			};
		}
	}

	#endregion
}
