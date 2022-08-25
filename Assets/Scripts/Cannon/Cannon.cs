using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	#region Properties

	[SerializeField]
	private float cannonHeight;
	[SerializeField]
	private float rotationTime;
	[SerializeField]
	private float heightRaiseTime;
	[SerializeField]
	private float loadAmmunitionTime;
	[SerializeField]
	private float fireAmmunitionTime;

	private float spawnHeight;
	private List<CannonShot> cannonShots;
	private GameObject cannonBallPrefab;

	#endregion

	#region Events

	private void Awake()
	{
		spawnHeight = -1;
		cannonBallPrefab = Resources.Load<GameObject>("Prefabs/Cannon/CannonBall");
	}

	#endregion

	#region Methods

	#region Get/Set

	public Vector3 GetSpawnHeightOffset()
	{
		return Vector3.up * spawnHeight;
	}

	#endregion

	public void SetupShot(List<CannonShot> shots)
	{
		cannonShots = shots;
		StartCoroutine(SetupShot());
	}

	private List<CannonBall> CreateCannonBalls(NumberCardSlot[] ammunitionSlots)
	{
		var attackOrbs      = new List<CannonBall>();

		foreach (var ammunitionSlot in ammunitionSlots)
		{
			var topCard = ammunitionSlot.GetTopCard();
			if (topCard == null) continue;

			var cannonBallPosition = ammunitionSlot.transform.position + GetSpawnHeightOffset();
			var cannonBallRotation = Quaternion.identity;
			var cannonBallObject   = Instantiate(cannonBallPrefab, cannonBallPosition, cannonBallRotation);
			var cannonBall         = cannonBallObject.GetComponent<CannonBall>();

			var cardValue    = topCard.GetCardType().GetValue().GetDescription();
			var cardSuit     = topCard.GetCardType().GetSuit();
			var damageAmount = Int32.Parse(cardValue);

			cannonBall.SetDamage(damageAmount, cardSuit);
			attackOrbs.Add(cannonBall);
		}

		return attackOrbs;
	}

	private CannonBall CombineCannonBalls(ref List<CannonBall> cannonBalls)
	{
		if (cannonBalls.Count > 2) return null;

		var cannonBallPosition = cannonBalls[0].transform.position;
		var cannonBallRotation = cannonBalls[0].transform.rotation;
		var cannonBallObject   = Instantiate(cannonBallPrefab, cannonBallPosition, cannonBallRotation);
		var cannonBall         = cannonBallObject.GetComponent<CannonBall>();

		cannonBall.SetDamage(cannonBalls[0], cannonBalls[1]);
		cannonBall.transform.localScale = cannonBallPrefab.transform.localScale * 2;

		foreach(var oldCannonBall in cannonBalls)
		{
			Destroy(oldCannonBall.gameObject);
		}
		cannonBalls.Clear();

		return cannonBall;
	}

	#endregion

	#region Coroutines

	public IEnumerator SetupShot()
	{
		foreach (var cannonShot in cannonShots)
		{
			// Rotate cannon towards target
			var targetPosition = cannonShot.targetSlot.transform.position;
			targetPosition.y   = transform.position.y;
			yield return StartCoroutine(RotateCannon(targetPosition));

			// Raise cannon and/or ammunition
			var ammunitionSlots = cannonShot.ammunitionSlots;
			var cannonBalls     = CreateCannonBalls(ammunitionSlots);
			yield return StartCoroutine(RaiseUpCannonAndAmmunition(cannonBalls));

			// Load ammunition
			var loadFinishPosition = transform.position;
			yield return StartCoroutine(MoveCannonBalls(cannonBalls, loadFinishPosition, loadAmmunitionTime));

			var cannonBall = CombineCannonBalls(ref cannonBalls);

			yield return new WaitForSeconds(0.5f);

			// Fire
			var fireFinishPosition = cannonShot.targetSlot.transform.position;
			fireFinishPosition.y   = cannonHeight;
			yield return StartCoroutine(MoveCannonBall(cannonBall, fireFinishPosition, fireAmmunitionTime));

			// Damage target and destroy
			var enemy        = cannonShot.targetSlot.GetEnemyForCard();
			var enemyHealth  = enemy.GetComponent<HealthSystem>();
			var damageAmount = cannonBall.GetDamageAmount();
			enemyHealth.TakeDamageOverTime(damageAmount, 1);

			Destroy(cannonBall.gameObject);
		}

		yield return new WaitForSeconds(0.5f);
		References.gameController.SetCurrentState(new GameStatePickupCard());
		Destroy(gameObject);
	}

	private IEnumerator RotateCannon(Vector3 targetPosition)
	{
		var currentRotation  = transform.rotation;
		var rotationToTarget = Quaternion.LookRotation((targetPosition - transform.position).normalized);
		var rotationTimer    = 0f;

		while (Quaternion.Angle(transform.rotation, rotationToTarget) > 1)
		{
			transform.rotation  = Quaternion.Lerp(currentRotation, rotationToTarget, rotationTimer / rotationTime);
			rotationTimer      += Time.deltaTime;

			yield return null;
		}
	}

	private IEnumerator RaiseUpCannonAndAmmunition(List<CannonBall> attackOrbs)
	{
		var timer     = 0f;
		var newHeight = spawnHeight;

		while (newHeight < cannonHeight)
		{
			// Cannon height
			newHeight = Mathf.Lerp(spawnHeight, cannonHeight, timer / heightRaiseTime);

			if (transform.position.y < cannonHeight)
			{
				// Only raise cannon for first cannonShot
				var newCannonPosition = transform.position;
				newCannonPosition.y   = newHeight;
				transform.position    = newCannonPosition;
			}

			// Attack orb height
			foreach (var attackOrb in attackOrbs)
			{
				var newOrbPosition           = attackOrb.transform.position;
				newOrbPosition.y             = newHeight;
				attackOrb.transform.position = newOrbPosition;
			}

			timer += Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator MoveCannonBalls(List<CannonBall> cannonBalls, Vector3 targetPosition, float timeToMove)
	{
		var timer = 0f;

		while (Vector3.Distance(cannonBalls[0].transform.position, targetPosition) > 0.1f)
		{
			foreach (var cannonBall in cannonBalls)
			{
				cannonBall.transform.position  = Vector3.Lerp(cannonBall.transform.position, targetPosition, timer / timeToMove);
				timer                         += Time.deltaTime;
				yield return null;
			}
		}
	}

	private IEnumerator MoveCannonBall(CannonBall cannonBall, Vector3 targetPosition, float timeToMove)
	{
		var timer = 0f;

		while (Vector3.Distance(cannonBall.transform.position, targetPosition) > 0.1f)
		{
			cannonBall.transform.position  = Vector3.Lerp(cannonBall.transform.position, targetPosition, timer / timeToMove);
			timer                         += Time.deltaTime;
			yield return null;
		}
	}

	#endregion
}
