using UnityEngine;
using static Element;

public class CannonBall : MonoBehaviour {
	#region Properties

	private int damageAmount;
	private ElementType damageType;

	private MeshRenderer meshRenderer;

	#endregion

	#region Events

	private void Awake() {
		meshRenderer = GetComponent<MeshRenderer>();
	}

	#endregion

	#region Methods

		#region Get/Set

		public int GetDamageAmount()
		{
			return damageAmount;
		}

		public void SetDamage(int amount, ElementType type) {
			damageAmount = amount;
			damageType = type;
			meshRenderer.material = Resources.Load<Material>($"Materials/CannonBall/{type}");
		}

		public void SetDamage(CannonBall firstCannonBall, CannonBall secondCannonBall) {
			var firstDamage   = firstCannonBall.damageAmount;
			var firstElement  = firstCannonBall.damageType;
			var secondDamage  = secondCannonBall.damageAmount;
			var secondElement = secondCannonBall.damageType;

			// If the elements are the same
			if (firstElement == secondElement)
			{
				SetDamage(firstCannonBall.damageAmount + secondCannonBall.damageAmount, firstElement);
				return;
			}

			// If the elements are opposites
			var firstElementPrimaryOpposite  = Element.GetOppositeElements(firstElement).Value.Item1;
			var secondElementPrimaryOpposite = Element.GetOppositeElements(secondElement).Value.Item1;

			if (firstElement == secondElementPrimaryOpposite ||
				secondElement == firstElementPrimaryOpposite)
			{
				Destroy(gameObject);
				return;
			}

			var amount = firstDamage > secondDamage ? firstDamage : secondDamage;
			var type   = Element.combinedElements[(firstElement, secondElement)];
			SetDamage(amount, type);
		}

		#endregion

	#endregion
}
