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

		public Vector3 GetSpawnHeightOffset() {
			return Vector3.up * spawnHeight;
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
			var targetPosition   = cannonShot.targetSlot.transform.position;
			targetPosition.y 	 = transform.position.y;
			var rotationToTarget = Quaternion.LookRotation((targetPosition - transform.position).normalized);
			var rotationTimer    = 0f;

			while(Quaternion.Angle(transform.rotation, rotationToTarget) > 1) {
				transform.rotation  = Quaternion.Lerp(currentRotation, rotationToTarget, rotationTimer / rotationTime);
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
				
				var attackOrbPosition = ammunitionSlot.transform.position + GetSpawnHeightOffset();
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
			var positionTimer  = 0f;
			var newHeight = spawnHeight;
			
			while(newHeight < cannonHeight) {
				// Cannon height
				newHeight = Mathf.Lerp(spawnHeight, cannonHeight, positionTimer / heightRaiseTime);

				if (transform.position.y < cannonHeight) {
					// Only raise cannon for first cannonShot
					var newCannonPosition = transform.position;
					newCannonPosition.y   = newHeight;
					transform.position    = newCannonPosition;
				}

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
			var loadFinishPosition  = transform.position;
			var loadAmmunitionTimer = 0f;

			while(Vector3.Distance(attackOrbs[0].transform.position, loadFinishPosition) > 0.1f) {
				foreach (var attackOrb in attackOrbs)
				{
					attackOrb.transform.position  = Vector3.Lerp(attackOrb.transform.position, loadFinishPosition, loadAmmunitionTimer / loadAmmunitionTime);
					loadAmmunitionTimer          += Time.deltaTime;
					yield return null;
				}
			}

			yield return new WaitForSeconds(0.5f);

			// Fire at target
			var fireFinishPosition  = cannonShot.targetSlot.transform.position;
			fireFinishPosition.y 	= cannonHeight;
			var fireAmmunitionTimer = 0f;

			while(Vector3.Distance(attackOrbs[0].transform.position, fireFinishPosition) > 0.1f) {
				foreach (var attackOrb in attackOrbs)
				{
					attackOrb.transform.position  = Vector3.Lerp(attackOrb.transform.position, fireFinishPosition, fireAmmunitionTimer / fireAmmunitionTime);
					fireAmmunitionTimer          += Time.deltaTime;
					yield return null;
				}
			}
		}
	}

	#endregion
}
