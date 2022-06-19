using System.Collections;
using static Card;

public class TempEnemyCardSlot : CardSlot {
	#region Events

	protected override void Awake() {
		base.Awake();

		References.Cards.Slots.tempEnemy = this;
	}

	#endregion

	#region Methods

	public Card PickupCard() {
		var card = RemoveCard();
		StartCoroutine(PickupCard(card));

		return card;
	}

	#endregion

	#region Coroutines

	private IEnumerator PickupCard(Card card) {
		var cardDeck       = References.Cards.cardDeck;
		var pickupPosition = cardDeck.GetPickupPosition();
		var pickupRotation = cardDeck.GetPickupRotation();
		card.GetComponent<EnemyCard>().SetStartGamePlacement(true);
		
		if (card.GetCurrentState() == CardState.Moving) {
			yield return null;
		}

		card.MoveCard(pickupPosition, pickupRotation, 1, CardState.PickedUp);

		while (card.GetCurrentState() == CardState.Moving) {
			// Wait until the card is fully picked up
			yield return null;
		}

		card.ActivateCard();
		card.GetComponent<EnemyCard>().SetStartGamePlacement(false);
	}

	#endregion
}
