using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Properties

	private EnemyState currentState;
	private EnemyCardSlot cardSlot;

	#endregion

	#region Events

	private void Awake()
	{
	}

	private void Update()
	{
		currentState.Update();
	}

	#endregion

	#region Methods

	#region Get/Set

	public void SetCurrentState(EnemyState state)
	{
		currentState = state;
	}

	public EnemyCardSlot GetCardSlot()
	{
		return cardSlot;
	}

	public void SetCardSlot(EnemyCardSlot enemyCardSlot)
	{
		cardSlot = enemyCardSlot;
	}

	#endregion

	#endregion
}
