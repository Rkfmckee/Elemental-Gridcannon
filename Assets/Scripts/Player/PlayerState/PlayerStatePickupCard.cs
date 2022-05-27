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

	protected override void LeftClickedHighlightable() {
		Debug.Log("Picked up card");
		playerController.SetCurrentState(new PlayerStatePlaceCard(gameObject));
	}

	#endregion
}
