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
	private Color defaultParticleColour;
	private Color translucentParticleColour;

	protected ParticleSystemRenderer[] particleSystems;
	private GameObject slotGlow;

	#endregion

	protected virtual void Awake() {
		slotGlow        = transform.Find("CardSlotGlow").gameObject;
		particleSystems = slotGlow.GetComponentsInChildren<ParticleSystemRenderer>();

		cards            = new Stack<Card>();
		nextCardPosition = transform.position;
		cardRotation     = new Vector3(0, -90, 0);

		defaultParticleColour       = particleSystems[0].material.GetColor("_TintColor");
		translucentParticleColour   = defaultParticleColour;
		translucentParticleColour.a = 0.1f;

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
			ShowCardSlot(canPlace);

			if (canPlace) References.Cards.Slots.active.Add(this);
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

	public virtual void AddCard(Card card) {
		card.MoveCard(nextCardPosition, cardRotation, 1, CardState.Placed);
		card.transform.parent = transform;
		card.currentSlot      = this;

		topCard = card;
		cards.Push(card);
		nextCardPosition.y += 0.01f;
	}

	public Card RemoveCard() {
		var removedCard                  = cards.Pop();
		    removedCard.transform.parent = null;
		    removedCard.currentSlot      = null;
		
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
			particleSystem.material.SetColor("_TintColor", defaultParticleColour);
		}
	}

	public void HighlightTranslucent() {
		foreach(var particleSystem in particleSystems) {
			particleSystem.material.SetColor("_TintColor", translucentParticleColour);
		}
	}

	protected void ShowCardSlot(bool show) {
		if (show) {
			slotGlow.transform.localPosition = Vector3.zero;
		}
		else
		{
			slotGlow.transform.localPosition = Vector3.down;
		}
	}

	#endregion
}
