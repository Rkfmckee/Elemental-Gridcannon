using UnityEngine;

public class EnemyCardSlot : CardSlot {
	#region Properties

	private NumberCardSlot adjacentNumberSlot;
	private Enemy enemyForCard;
	private Color targetColour;

	#endregion
	
	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.enemy.Add(this);

		targetColour = Color.red;
	}

	#endregion

	#region Methods

		#region Get/Set

		public NumberCardSlot GetAdjacentNumberSlot() {
			return adjacentNumberSlot;
		}

		public void SetAdjacentNumberSlot(NumberCardSlot slot) {
			adjacentNumberSlot = slot;
		}

		public Enemy GetEnemyForCard()
		{
			return enemyForCard;
		}

	#endregion

	public override void AddCard(Card card)
	{
		base.AddCard(card);

		var enemyCard = card.GetComponent<EnemyCard>();
		if (enemyCard == null) {
			return;
		}

		var enemy = enemyCard.SpawnEnemy();
		enemyForCard = enemy;
	}

	public void HighlightTarget() {
		ShowCardSlot(true);

		foreach(var particleSystem in particleSystems) {
			particleSystem.material.SetColor("_TintColor", targetColour);
		}
	}

	public void UnhighlightTarget() {
		ShowCardSlot(false);

		HighlightTranslucent();
	}

	#endregion
}
