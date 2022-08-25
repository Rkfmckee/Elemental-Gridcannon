using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Card;
using static CardType;
using static Element;

public class CardDeck : MonoBehaviour {
	#region Properties

	private List<(ElementType, CardValue)> cardsInDeck;
	private Vector3 spawnPosition;
	private Vector3 outOfBoxPosition;
	private Vector3 pickupPosition;
	private Vector3 pickupRotation;
	private Vector3 spawnRotation;

	#endregion

	#region Events

	private void Awake() {
		References.Cards.cardDeck = this;

		spawnPosition    = new Vector3(0, 0.1f, 0);
		outOfBoxPosition = new Vector3(6, 0, 1);
		pickupPosition   = new Vector3(2, 2, -2.9f);
		pickupRotation   = new Vector3(-30, 0, 10);
		spawnRotation    = new Vector3(0, 0, -180);

		FillDeckWithCards();
	}

	#endregion
	
	#region Methods

		#region Get/Set

		public Vector3 GetPickupPosition() {
			return pickupPosition;
		}

		public Vector3 GetPickupRotation() {
			return pickupRotation;
		}

		#endregion

	public void LayoutInitialCards() {
		StartCoroutine(LayoutCards());
	}

	public bool PickupCard() {
		if (cardsInDeck.Count == 0) {
			print("No cards remaining in deck");
			return false;
		}

		var card = SpawnNextCard();
		StartCoroutine(PickupCard(card));
		References.Cards.currentCard = card;

		return true;
	}

	private void FillDeckWithCards() {
		cardsInDeck   = new List<(ElementType, CardValue)>();
		var allValues = Enum.GetValues(typeof(CardValue));
		var allSuits  = new[] { ElementType.Air, ElementType.Earth, ElementType.Fire, ElementType.Water };

		foreach(var cardSuit in allSuits) {
			foreach(var value in allValues) {
				var cardValue = (CardValue) value;
				cardsInDeck.Add((cardSuit, cardValue));
			}
		}

		var random     = new System.Random();
		var randomized = cardsInDeck.OrderBy(item => random.Next());
		cardsInDeck    = randomized.ToList();
	}

	private Card SpawnNextCard() {
		var cardToPick = cardsInDeck[0];
		var cardSuit   = cardToPick.Item1;
		var cardValue  = cardToPick.Item2;
		cardsInDeck.RemoveAt(0);
		
		var cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardSuit}/{cardValue.GetDescription()}{cardSuit}");
		var card       = Instantiate(cardPrefab);

		card.transform.position    = transform.position + spawnPosition;
		card.transform.eulerAngles = spawnRotation;

		var cardController = card.GetComponent<Card>();
		cardController.SetCardType(cardValue, cardSuit);

		return cardController;
	}

	#endregion

	#region Coroutines

	private IEnumerator LayoutCards() {
		var numberCardSlots   = References.Cards.Slots.number;
		var specialCardSlot   = References.Cards.Slots.special;
		var tempEnemyCardSlot = References.Cards.Slots.tempEnemy;

		int i = 0;
		while(i < numberCardSlots.Count) {
			var slot = numberCardSlots[i];

			if (slot.GetAdjacentEnemySlots().Count == 0) {
				i++;
				continue;
			}

			var card = SpawnNextCard();
			if (card is EnemyCard) {
				tempEnemyCardSlot.AddCard(card);
			} else if (card is SpecialCard) {
				specialCardSlot.AddCard(card);
			} else {
				slot.AddCard(card);
				i++;
			}

			yield return new WaitForSeconds(0.2f);
		}

		yield return new WaitForSeconds(1);

		if (tempEnemyCardSlot.GetCards().Count > 0) {
			References.gameController.SetCurrentState(new GameStatePlaceStartingEnemies());
		} else {
			References.gameController.SetCurrentState(new GameStatePickupCard());
		}
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

	#endregion
}
