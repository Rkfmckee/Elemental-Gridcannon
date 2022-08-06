using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Properties

	private EnemyCardSlot cardSlot;

	#endregion

	#region Get/Set Methods

	public EnemyCardSlot GetCardSlot() {
		return cardSlot;
	}

	public void SetCardSlot(EnemyCardSlot enemyCardSlot) {
		cardSlot = enemyCardSlot;
	}

	#endregion
}
