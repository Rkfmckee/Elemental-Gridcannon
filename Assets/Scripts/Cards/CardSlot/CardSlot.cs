using System.Collections.Generic;
using UnityEngine;
using static Card;

public abstract class CardSlot : MonoBehaviour {
	#region Properties

	private bool canPlaceCard;
	private Card topCard;
	private Stack<Card> cards;
	private Vector3 nextCardPosition;
	private Vector3 cardRotation;
	private Color defaultParticleMaterialColour;
	private Color translucentParticleMaterialColour;

	private GameObject slotGlow;
	private ParticleSystemRenderer[] particleSystems;

	#endregion

	protected virtual void Awake() {
		slotGlow = transform.Find("CardSlotGlow").gameObject;
		particleSystems = slotGlow.GetComponentsInChildren<ParticleSystemRenderer>();

		cards = new Stack<Card>();
		nextCardPosition = transform.position;
		cardRotation = new Vector3(0, -90, 0);

		defaultParticleMaterialColour = particleSystems[0].material.GetColor("_TintColor");
		translucentParticleMaterialColour = defaultParticleMaterialColour;
		translucentParticleMaterialColour.a = 0.1f;

		SetCanPlaceCard(false);
		HighlightTranslucent();
	}
	
	#region Methods

		#region Get/Set

		public bool CanPlaceCard() {
			return canPlaceCard;
		}

		public void SetCanPlaceCard(bool canPlace) {
			canPlaceCard = canPlace;

			if (canPlace) {
				slotGlow.transform.localPosition = Vector3.zero;
				References.Cards.Slots.active.Add(this);
			} else {
				slotGlow.transform.localPosition = Vector3.down;
			}
		}

		public Card GetTopCard() {
			return topCard;
		}

		public void SetTopCard(Card card) {
			topCard = card;
		}

		public Stack<Card> GetCards() {
			return cards;
		}

		#endregion

	public void AddCard(Card card) {
		card.MoveCard(nextCardPosition, cardRotation, 1, CardState.Placed);
		card.transform.parent = transform;
		card.currentSlot = this;

		topCard = card;
		cards.Push(card);
		nextCardPosition.y += 0.01f;

		var enemyCard = card.GetComponent<EnemyCard>();
		if (enemyCard != null && this is EnemyCardSlot) {
			enemyCard.SpawnEnemy();
		}
	}

	public Card RemoveCard() {
		var removedCard = cards.Pop();
		removedCard.transform.parent = null;
		removedCard.currentSlot = null;
		
		if (cards.Count > 0) {
			topCard = cards.Peek();
		} else {
			topCard = null;
		}

		nextCardPosition.y -= 0.01f;
		return removedCard;
	}

	public void HighlightDefault() {
		foreach(var particleSystem in particleSystems) {
			particleSystem.material.SetColor("_TintColor", defaultParticleMaterialColour);
		}
	}

	public void HighlightTranslucent() {
		foreach(var particleSystem in particleSystems) {
			particleSystem.material.SetColor("_TintColor", translucentParticleMaterialColour);
		}
	}

	#endregion
}
