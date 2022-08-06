using UnityEngine;

public class AttackOrb : MonoBehaviour {
	#region Properties

	private int damageAmount;
	private DamageType damageType;

	private MeshRenderer meshRenderer;

	#endregion

	#region Events

	private void Awake() {
		meshRenderer = GetComponent<MeshRenderer>();
	}

	#endregion

	#region Methods

		#region Get/Set

		public void SetDamage(int amount, DamageType type) {
			damageAmount = amount;
			damageType = type;
			meshRenderer.material = Resources.Load<Material>($"Materials/AttackOrb/{type}");
		}

		#endregion

	#endregion

	#region Enums

	public enum DamageType {
		Air,
		Earth,
		Fire,
		Water
	}

	#endregion
}
