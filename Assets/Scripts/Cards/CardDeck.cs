using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Card;
using static CardType;

public class CardDeck : MonoBehaviour {
	#region Properties

	private List<(CardSuit, CardValue)> cardsInDeck;
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

		FillDeckWithCards();
	}

	#endregion
	
	#region Methods
	public bool PickupCard() {
		if (cardsInDeck.Count == 0) {
			print("No cards remaining in deck");
			return false;
		}

		var cardToPick = cardsInDeck[0];
		cardsInDeck.RemoveAt(0);
		var cardSuit = cardToPick.Item1;
		var cardValue = cardToPick.Item2;
		
		var cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardSuit}/{cardValue.GetDescription()}{cardSuit}");
		var card = Instantiate(cardPrefab);
		card.transform.position = transform.position + topOfDeckPosition;
		card.transform.eulerAngles = new Vector3(0, 0, -180);

		var cardController = card.GetComponent<Card>();
		cardController.SetCardType(cardValue, cardSuit);

		StartCoroutine(PickupCard(cardController));

		References.Cards.currentCard = cardController;
		return true;
	}

	private IEnumerator PickupCard(Card card) {
		card.MoveCard(pickupPosition, pickupRotation, 1, CardState.PickedUp);

		while (card.GetCurrentState() == CardState.Moving) {
			// Wait until the card is fully picked up
			yield return null;
		}

		card.ActivateCard();
	}

	private void FillDeckWithCards() {
		cardsInDeck = new List<(CardSuit, CardValue)>();
		var allValues = Enum.GetValues(typeof(CardValue));
		var allSuits = Enum.GetValues(typeof(CardSuit));

		foreach(var suit in allSuits) {
			var cardSuit = (CardSuit) suit;
			foreach(var value in allValues) {
				var cardValue = (CardValue) value;
				cardsInDeck.Add((cardSuit, cardValue));
			}
		}

		var random = new System.Random();
        var randomized = cardsInDeck.OrderBy(item => random.Next());
		cardsInDeck = randomized.ToList();
	}

	#endregion
}
