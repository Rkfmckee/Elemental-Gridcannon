using UnityEngine;

public class PlayerStatePlaceCard : PlayerState {
	#region Constructor

	public PlayerStatePlaceCard(GameObject gameObj) : base(gameObj) {
		highlightMask = 1 << LayerMask.NameToLayer("CardSlot");
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();
	}

	#endregion

	#region Methods

	protected override void LeftClickedHighlightable() {
		Debug.Log("Placed card");
		playerController.SetCurrentState(new PlayerStatePickupCard(gameObject));
	}

	#endregion
}
