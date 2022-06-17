using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardType;

public class EnemyCard : Card {
	#region Methods

	public override void ActivateCard() {
		var oppositeSuits         = cardType.GetOppositeSuits();
		var primaryOppositeSuit   = oppositeSuits.Value.Item1;
		var secondaryOppositeSuit = oppositeSuits.Value.Item2;

		var numberSlots  = References.Cards.Slots.number;
		var slotsForType = new Dictionary<CardSuit, List<NumberCardSlot>> {
			{ CardSuit.Air, new List<NumberCardSlot>() },
			{ CardSuit.Earth, new List<NumberCardSlot>() },
			{ CardSuit.Fire, new List<NumberCardSlot>() },
			{ CardSuit.Water, new List<NumberCardSlot>() },
		};
		var validEnemySlots = new List<EnemyCardSlot>();

		foreach(var slot in numberSlots) {
			if (slot.GetCards().Count == 0) {
				continue;
			}

			var topCardSuit = slot.GetTopCard().GetCardType().GetSuit();
			slotsForType[topCardSuit].Add(slot);
		}

		validEnemySlots = GetHighestValue(slotsForType[primaryOppositeSuit]);

		if (validEnemySlots.Count == 0) {
			validEnemySlots = GetHighestValue(slotsForType[secondaryOppositeSuit]);
		}

		if (validEnemySlots.Count == 0) {
			validEnemySlots = GetHighestValue(numberSlots);
		}

		if (validEnemySlots.Count == 0) {
			validEnemySlots = References.Cards.Slots.enemy;
		}

		if (validEnemySlots.Count == 1) {
			var validSlot = validEnemySlots[0];

			if (validSlot.GetCards().Count == 0) {
				validSlot.AddCard(this);
				References.playerController.SetCurrentState(new PlayerStatePickupCard());
				return;
			}
		}

		ActivateValidSlots(validEnemySlots);
	}

	public void SpawnEnemy() {
		StartCoroutine(ShrinkCardAndSpawnEnemy());
	}

	private List<EnemyCardSlot> GetHighestValue(List<NumberCardSlot> slots) {
		var validEnemySlots = new List<EnemyCardSlot>();
		
		if (slots.Count == 0) {
			return validEnemySlots;
		}
		
		CardValue? highestSlotValue = null;

		// First make sure at least one of the slots has a card
		foreach(var slot in slots) {
			if (ShouldIgnoreSlot(slot)) {
				continue;
			}

			if (slot.GetCards().Count > 0) {
				highestSlotValue = (CardValue?) slot.GetTopCard().GetCardType().GetValue();
				break;
			}
		}

		// If all the slots are empty
		if (!highestSlotValue.HasValue) {
			return validEnemySlots;
		}

		// Otherwise, at least one slot has a card
		foreach(var slot in slots) {
			if (ShouldIgnoreSlot(slot)) {
				continue;
			}

			if (slot.GetCards().Count == 0) {
				continue;
			}

			var cardValue  = slot.GetTopCard().GetCardType().GetValue();
			var validSlots = GetEmptyEnemySlots(slot);
			
			if (cardValue > highestSlotValue) {
				highestSlotValue = cardValue;
				validEnemySlots.Clear();
				validEnemySlots.AddRange(validSlots);
			} else if (cardValue == highestSlotValue) {
				validEnemySlots.AddRange(validSlots);
			}
		}

		return validEnemySlots;
	}

	private bool ShouldIgnoreSlot(NumberCardSlot slot) {
		var enemySlots = slot.GetAdjacentEnemySlots();
		
		// Ignore slots an enemy can't be placed beside, like the middle one
		if (enemySlots.Count == 0) {
			return true;
		}


		if (GetEmptyEnemySlots(slot).Count == 0) {
			return true;
		}

		return false;
	}

	private List<EnemyCardSlot> GetEmptyEnemySlots(NumberCardSlot slot) {
		var emptySlots = new List<EnemyCardSlot>();

		foreach(var enemySlot in slot.GetAdjacentEnemySlots()) {
			if (enemySlot.GetCards().Count == 0) {
				emptySlots.Add(enemySlot);
			}
		}

		return emptySlots;
	}

	private void ActivateValidSlots(List<EnemyCardSlot> validSlots) {
		foreach(var slot in validSlots) {
			if (slot.GetCards().Count == 0) {
				slot.SetCanPlaceCard(true);
			}
		}

		References.playerController.SetCurrentState(new PlayerStatePlaceCard());
	}

	#endregion

	#region Coroutines

	private IEnumerator ShrinkCardAndSpawnEnemy() {
		var enemyPrefab   = Resources.Load<GameObject>($"Prefabs/Enemy/{cardType.GetSuit()}Golem");
		var enemyCardSlot = currentSlot.GetComponent<EnemyCardSlot>();
		var enemyFacing   = (currentSlot.transform.position - enemyCardSlot.GetAdjacentNumberSlot().transform.position).normalized;
		var shrinkRate    = 1;
		var shrinkTarget  = 0.2f;
		var growTarget    = enemyPrefab.transform.localScale.magnitude;
		
		while (GetCurrentState() == CardState.Moving) {
			yield return null;
		}

		while (transform.localScale.magnitude > shrinkTarget) {
			transform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;
			yield return null;
		}

		var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
		enemy.transform.localScale = Vector3.one * shrinkTarget;
		enemy.transform.rotation = Quaternion.LookRotation(enemyFacing);

		while(enemy.transform.localScale.magnitude < growTarget ) {
			enemy.transform.localScale += Vector3.one * shrinkRate * Time.deltaTime;
			yield return null;
		}

		enemy.transform.localScale = Vector3.one;
		Destroy(gameObject);
	}

	#endregion
}
