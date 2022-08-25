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

	#endregion

	#region Events

	private void Awake()
	{
		spawnHeight = -1;
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

	private List<AttackOrb> CreateAttackOrbs(NumberCardSlot[] ammunitionSlots)
	{
		var attackOrbPrefab = Resources.Load<GameObject>("Prefabs/Cannon/AttackOrb");
		var attackOrbs      = new List<AttackOrb>();

		foreach (var ammunitionSlot in ammunitionSlots)
		{
			var topCard = ammunitionSlot.GetTopCard();
			if (topCard == null) continue;

			var attackOrbPosition = ammunitionSlot.transform.position + GetSpawnHeightOffset();
			var attackOrbRotation = Quaternion.identity;
			var attackOrbObject   = Instantiate(attackOrbPrefab, attackOrbPosition, attackOrbRotation);
			var attackOrb         = attackOrbObject.GetComponent<AttackOrb>();

			var cardValue    = topCard.GetCardType().GetValue().GetDescription();
			var cardSuit     = topCard.GetCardType().GetSuit();
			var damageAmount = Int32.Parse(cardValue);

			attackOrb.SetDamage(damageAmount, cardSuit);
			attackOrbs.Add(attackOrb);
		}

		return attackOrbs;
	}

	private int GetAttackDamage(CannonShot cannonShot)
	{
		var totalDamage = 0;

		foreach (var ammunitionSlot in cannonShot.ammunitionSlots)
		{
			var topCard = ammunitionSlot.GetTopCard();
			if (topCard == null) continue;

			totalDamage += Int32.Parse(topCard.GetCardType().GetValue().GetDescription());
		}

		return totalDamage;
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
			var attackOrbs      = CreateAttackOrbs(ammunitionSlots);
			yield return StartCoroutine(RaiseUpCannonAndAmmunition(attackOrbs));

			// Load ammunition
			var loadFinishPosition = transform.position;
			yield return StartCoroutine(MoveOrbPosition(attackOrbs, loadFinishPosition, loadAmmunitionTime));

			yield return new WaitForSeconds(0.5f);

			// Fire
			var fireFinishPosition = cannonShot.targetSlot.transform.position;
			fireFinishPosition.y   = cannonHeight;
			yield return StartCoroutine(MoveOrbPosition(attackOrbs, fireFinishPosition, fireAmmunitionTime));

			// Damage target and destroy
			var enemy        = cannonShot.targetSlot.GetEnemyForCard();
			var enemyHealth  = enemy.GetComponent<HealthSystem>();
			var damageAmount = GetAttackDamage(cannonShot);
			enemyHealth.TakeDamageOverTime(damageAmount, 1);

			foreach (var attackOrb in attackOrbs)
			{
				Destroy(attackOrb.gameObject);
			}
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

	private IEnumerator RaiseUpCannonAndAmmunition(List<AttackOrb> attackOrbs)
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

	private IEnumerator MoveOrbPosition(List<AttackOrb> attackOrbs, Vector3 targetPosition, float timeToMove)
	{
		var timer = 0f;

		while (Vector3.Distance(attackOrbs[0].transform.position, targetPosition) > 0.1f)
		{
			foreach (var attackOrb in attackOrbs)
			{
				attackOrb.transform.position  = Vector3.Lerp(attackOrb.transform.position, targetPosition, timer / fireAmmunitionTime);
				timer                        += Time.deltaTime;
				yield return null;
			}
		}
	}

	#endregion
}
