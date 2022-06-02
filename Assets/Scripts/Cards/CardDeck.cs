using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;
using static CardType;

public class CardDeck : MonoBehaviour {
	#region Properties

	private Vector3 topOfDeckPosition;
	private Vector3 pickupPosition;
	private Vector3 pickupRotation;
	private List<CardValue> enemyValues;
	private List<CardValue> specialValues;

	#endregion

	#region Events

	private void Awake() {
		topOfDeckPosition = new Vector3(0, 0.2f, 0);
		pickupPosition = new Vector3(1.5f, 2, -2.5f);
		pickupRotation = new Vector3(-30, 0, 0);

		enemyValues = new List<CardValue> { CardValue.Jack, CardValue.Queen, CardValue.King };
		specialValues = new List<CardValue> { CardValue.Ace };
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

		StartCoroutine(PickupCard(cardController));

		References.Cards.currentCard = cardController;
	}

	private IEnumerator PickupCard(Card card) {
		card.MoveCard(pickupPosition, pickupRotation, 1, CardState.PickedUp);

		while (card.GetCurrentState() == CardState.Moving) {
			// Wait until the card is fully picked up
			yield return null;
		}

		card.ActivateCard();
	} 

	#endregion
}
