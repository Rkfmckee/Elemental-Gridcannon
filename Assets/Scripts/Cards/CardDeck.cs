using UnityEngine;
using static Card;

public class CardDeck : MonoBehaviour {
	#region Properties

	private GameObject cardPrefab;
	private Vector3 topOfDeckPosition;
	private Vector3 pickupPosition;
	private Vector3 pickupRotation;

	#endregion

	#region Events

	private void Awake() {
		cardPrefab = Resources.Load<GameObject>("Prefabs/Card");
		topOfDeckPosition = new Vector3(0, 0.2f, 0);
		pickupPosition = new Vector3(1.5f, 2, -2.5f);
		pickupRotation = new Vector3(-30, 0, 0);
	}

	#endregion
	
	#region Methods

	public void PickupCard() {
		var card = Instantiate(cardPrefab);
		card.transform.position = transform.position + topOfDeckPosition;
		card.transform.eulerAngles = new Vector3(0, 0, -180);

		var cardController = card.GetComponent<Card>();
		cardController.MoveCard(pickupPosition, pickupRotation, 1, CardState.PickedUp);

		References.Cards.currentCard = cardController;
	}

	#endregion
}
