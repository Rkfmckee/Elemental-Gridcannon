using UnityEngine;
using static Card;

public class PlayerStatePlaceCard : PlayerState {
	#region Properties

	private Card card;

	#endregion
	
	#region Constructor

	public PlayerStatePlaceCard(GameObject gameObj) : base(gameObj) {
		highlightMask = 1 << LayerMask.NameToLayer("CardSlot");
		card = References.Cards.currentCard;

		var cardSlots = References.Cards.cardSlots;
		foreach(var slot in cardSlots) {
			slot.SetGlowEnabled(true);
		}
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();
	}

	#endregion

	#region Methods

	public override void CleanupState() {
		base.CleanupState();

		Debug.Log("here 0");
		var cardSlots = References.Cards.cardSlots;
		foreach(var slot in cardSlots) {
			slot.SetGlowEnabled(false);
			Debug.Log("here 1");
		}
	}

	protected override void LeftClicked(GameObject target) {
		var cardSlot = target.GetComponent<CardSlot>();
		cardSlot.AddCard(card);
		
		playerController.SetCurrentState(new PlayerStatePickupCard(gameObject));
	}

	protected override void EnableHighlight(GameObject target) {
		target.GetComponent<CardSlot>().HighlightDefault();
	}

	protected override void DisableHighlight(GameObject target) {
		target.GetComponent<CardSlot>().HighlightTranslucent();
	}

	protected override void LookForHighlightable() {
		if (card.GetCurrentState() == CardState.Moving) {
			return;
		}

		base.LookForHighlightable();
	}

	#endregion
}
