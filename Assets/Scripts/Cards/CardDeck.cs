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
	private Vector3 spawnPosition;
	private Vector3 outOfBoxPosition;
	private Vector3 pickupPosition;
	private Vector3 pickupRotation;
	private Vector3 spawnRotation;

	#endregion

	#region Events

	private void Awake() {
		spawnPosition = new Vector3(0, 0.1f, 0);
		outOfBoxPosition = new Vector3(5, 0, 0);
		pickupPosition = new Vector3(1.4f, 3, -2.5f);
		pickupRotation = new Vector3(-30, 0, 10);
		spawnRotation = new Vector3(0, 0, -180);

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
		card.transform.position = transform.position + spawnPosition;
		card.transform.eulerAngles = spawnRotation;

		var cardController = card.GetComponent<Card>();
		cardController.SetCardType(cardValue, cardSuit);

		StartCoroutine(PickupCard(cardController));

		References.Cards.currentCard = cardController;
		return true;
	}

	private IEnumerator PickupCard(Card card) {
		card.MoveCard(outOfBoxPosition, spawnRotation, 0.5f, CardState.Stationary);

		while (card.GetCurrentState() == CardState.Moving) {
			// Wait until the card is out of box
			yield return null;
		}

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
