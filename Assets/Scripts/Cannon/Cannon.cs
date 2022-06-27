using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackOrb;

public class Cannon : MonoBehaviour
{
	#region Properties

	[SerializeField]
	private float cannonHeight;
	[SerializeField]
	private float timeToRotate;
	[SerializeField]
	private float timeToPosition;

	private Vector3 spawnHeightOffset;
	private List<CannonShot> cannonShots;

	#endregion

	#region Events

	private void Awake()
	{
		spawnHeightOffset = Vector3.down;
	}

	#endregion

	#region Methods

		#region Get/Set

		public Vector3 GetSpawnHeightOffset() {
			return spawnHeightOffset;
		}

		#endregion

	public void SetupShot(List<CannonShot> shots)
	{
		cannonShots = shots;
		StartCoroutine(SetupShot());
	}

	#endregion

	#region Coroutines

	public IEnumerator SetupShot()
	{
		// For each cannon shot
		foreach (var cannonShot in cannonShots)
		{
			// Rotate cannon towards correct target
			var currentRotation  = transform.rotation;
			var targetPosition = cannonShot.targetSlot.transform.position + spawnHeightOffset;
			var rotationToTarget = Quaternion.LookRotation((targetPosition - transform.position).normalized);
			var rotationTimer    = 0f;

			while(Quaternion.Angle(transform.rotation, rotationToTarget) > 1) {
				transform.rotation  = Quaternion.Lerp(currentRotation, rotationToTarget, rotationTimer / timeToRotate);
				rotationTimer      += Time.deltaTime;

				yield return null;
			}

			// Create ammunition shots and set damage type and value
			var attackOrbPrefab = Resources.Load<GameObject>("Prefabs/Cannon/AttackOrb");
			var attackOrbs      = new List<AttackOrb>();

			foreach (var ammunitionSlot in cannonShot.ammunitionSlots)
			{
				var topCard = ammunitionSlot.GetTopCard();
				if (topCard == null) continue;
				
				var attackOrbPosition = ammunitionSlot.transform.position + spawnHeightOffset;
				var attackOrbRotation = Quaternion.identity;
				var attackOrbObject   = Instantiate(attackOrbPrefab, attackOrbPosition, attackOrbRotation);
				var attackOrb         = attackOrbObject.GetComponent<AttackOrb>();
				
				var cardValue    = topCard.GetCardType().GetValue().GetDescription();
				var cardSuit     = topCard.GetCardType().GetSuit().GetDescription();
				var damageAmount = Int32.Parse(cardValue);
				var damageType   = (DamageType) Enum.Parse(typeof(DamageType), cardSuit);

				attackOrb.SetDamage(damageAmount, damageType);
				attackOrbs.Add(attackOrb);
			}

			// Raise up cannon and ammunition if needed
			var startingHeight = transform.position.y;
			var positionTimer  = 0f;
			
			while(transform.position.y < cannonHeight) {
				// Cannon height
				var newHeight         = Mathf.Lerp(startingHeight, cannonHeight, positionTimer / timeToPosition);
				var newCannonPosition = transform.position;
				newCannonPosition.y   = newHeight;
				transform.position    = newCannonPosition;

				// Attack orb height
				foreach (var attackOrb in attackOrbs)
				{
					var newOrbPosition 			 = attackOrb.transform.position;
					newOrbPosition.y   			 = newHeight;
					attackOrb.transform.position = newOrbPosition;
				}

				positionTimer += Time.deltaTime;
				yield return null;
			}

			// Bring ammunition back towards cannon

			// Fire at target
		}
	}

	#endregion
}
