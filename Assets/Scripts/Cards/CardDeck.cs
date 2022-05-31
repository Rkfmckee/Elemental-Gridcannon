using System;
using UnityEngine;
using static Card;
using static CardType;

public class CardDeck : MonoBehaviour {
	#region Properties

	private Vector3 topOfDeckPosition;
	private Vector3 pickupPosition;
	private Vector3 pickupRotation;

	#endregion

	#region Events

	private void Awake() {
		topOfDeckPosition = new Vector3(0, 0.2f, 0);
		pickupPosition = new Vector3(1.5f, 2, -2.5f);
		pickupRotation = new Vector3(-30, 0, 0);
	}

	#endregion
	
	#region Methods

	public void PickupCard() {
		var allValues = Enum.GetValues(typeof(CardValue));
		var cardValue = (CardValue) allValues.GetValue(UnityEngine.Random.Range(0, allValues.Length));

		var allSuits = Enum.GetValues(typeof(CardSuit));
		var cardSuit = (CardSuit) allSuits.GetValue(UnityEngine.Random.Range(0, allSuits.Length));
		
		var cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardSuit}/{cardValue.GetDescription()}{cardSuit}");
		var card = Instantiate(cardPrefab);
		card.transform.position = transform.position + topOfDeckPosition;
		card.transform.eulerAngles = new Vector3(0, 0, -180);

		var cardController = card.GetComponent<Card>();
		cardController.SetCardType(cardValue, cardSuit);
		cardController.MoveCard(pickupPosition, pickupRotation, 1, CardState.PickedUp);

		References.Cards.currentCard = cardController;
	}

	#endregion
}
