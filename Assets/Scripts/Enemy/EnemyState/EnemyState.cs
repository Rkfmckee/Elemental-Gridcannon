using UnityEngine;

public abstract class EnemyState
{
	#region Properties

	protected GameObject gameObject;

	#endregion

	#region Constructors

	public EnemyState(GameObject gameObj) 
	{
		gameObject = gameObj;
	}

	#endregion
	
	#region Events

	public virtual void Update()
	{
	}

	#endregion
}
