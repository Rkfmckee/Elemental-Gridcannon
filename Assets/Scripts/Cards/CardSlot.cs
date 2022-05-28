using UnityEngine;
using static Card;

public class CardSlot : MonoBehaviour {
	#region Methods

	public void AddCard(Card card) {
		card.MoveCard(transform.position, transform.eulerAngles, 1, CardState.Placed);
		card.transform.parent = transform;
	}

	#endregion
}
