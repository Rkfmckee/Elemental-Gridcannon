using System.Collections.Generic;
using UnityEngine;
using static Card;

public abstract class CardSlot : MonoBehaviour {
	#region Properties

	private bool canPlaceCard;
	private List<Card> cards;
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

		canPlaceCard = false;
		cards = new List<Card>();
		nextCardPosition = transform.position;
		cardRotation = new Vector3(0, -90, 0);

		defaultParticleMaterialColour = particleSystems[0].material.GetColor("_TintColor");
		translucentParticleMaterialColour = defaultParticleMaterialColour;
		translucentParticleMaterialColour.a = 0.1f;

		HighlightTranslucent();
	}
	
	#region Methods

		#region Get/Set

		public bool CanPlaceCard() {
			return canPlaceCard;
		}

		public void SetCanPlaceCard(bool canPlace) {
			canPlaceCard = canPlace;
			slotGlow.SetActive(canPlace);
		}

		#endregion

	public void AddCard(Card card) {
		card.MoveCard(nextCardPosition, cardRotation, 1, CardState.Placed);
		card.transform.parent = transform;

		cards.Add(card);
		nextCardPosition.y += 0.01f;
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
