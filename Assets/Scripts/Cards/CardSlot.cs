using System.Collections.Generic;
using UnityEngine;
using static Card;
using static UnityEngine.ParticleSystem;

public class CardSlot : MonoBehaviour {
	#region Properties

	private List<Card> cards;
	private Vector3 nextCardPosition;
	private Color defaultParticleMaterialColour;
	private Color translucentParticleMaterialColour;

	private GameObject slotGlow;
	private ParticleSystemRenderer[] particleSystems;

	#endregion

	private void Awake() {
		References.Cards.cardSlots.Add(this);
		slotGlow = transform.Find("CardSlotGlow").gameObject;
		particleSystems = slotGlow.GetComponentsInChildren<ParticleSystemRenderer>();

		cards = new List<Card>();
		nextCardPosition = transform.position;

		defaultParticleMaterialColour = particleSystems[0].material.GetColor("_TintColor");
		translucentParticleMaterialColour = defaultParticleMaterialColour;
		translucentParticleMaterialColour.a = 0.1f;

		HighlightTranslucent();
	}
	
	#region Methods

	public void AddCard(Card card) {
		card.MoveCard(nextCardPosition, transform.eulerAngles, 1, CardState.Placed);
		card.transform.parent = transform;

		cards.Add(card);
		nextCardPosition.y += card.transform.localScale.y;
	}

	public void SetGlowEnabled(bool enable) {
		slotGlow.SetActive(enable);
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
