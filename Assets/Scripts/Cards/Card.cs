using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour {
	#region Properties

	private CardState currentState;
	private float movementTimer;

	#endregion

	#region Methods

		#region Get/Set

		public CardState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(CardState state) {
			currentState = state;
		}

		#endregion

	public void MoveCard(Vector3 position, Vector3 rotation, float timeToMove, CardState finishState) {
		StartCoroutine(MovePositionAndRotation(transform.position, position, Quaternion.Euler(rotation), timeToMove, finishState));
	}

	private IEnumerator MovePositionAndRotation(Vector3 startPosition, Vector3 finishPosition, Quaternion finishRotation, float timeToMove, CardState finishState) {
		currentState = CardState.Moving;
		var startRotation = transform.rotation;
		
		while (movementTimer < timeToMove) {
			transform.position = Vector3.Lerp(startPosition, finishPosition, movementTimer / timeToMove);
			transform.rotation = Quaternion.Lerp(startRotation, finishRotation, movementTimer / timeToMove);

			movementTimer += Time.deltaTime;
			yield return null;
		}

		transform.position = finishPosition;
		transform.rotation = finishRotation;
	
		currentState = finishState;
		movementTimer = 0;
	}

	#endregion

	#region Enums

	public enum CardState {
		Moving,
		PickedUp,
		Placed
	}

	#endregion
}