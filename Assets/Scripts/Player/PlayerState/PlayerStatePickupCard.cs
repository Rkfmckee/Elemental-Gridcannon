using UnityEngine;

public class PlayerStatePickupCard : PlayerState {
	#region Constructor

	public PlayerStatePickupCard(GameObject gameObj) : base(gameObj) {
		highlightMask = 1 << LayerMask.NameToLayer("CardDeck");
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();
	}

	#endregion

	#region Methods

	protected override void LeftClicked(GameObject target) {
		var cardDeck = target.GetComponent<CardDeck>();
		cardDeck.PickupCard();

		playerController.SetCurrentState(new PlayerStatePlaceCard(gameObject));
	}

	#endregion
}
