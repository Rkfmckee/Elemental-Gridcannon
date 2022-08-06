using UnityEngine;

public class EnemyStateIdle : EnemyState
{
	#region Properties

	private Vector3 basePosition;

	private GameController gameController;

	#endregion

	#region Constructors

	public EnemyStateIdle(GameObject gameObj) : base(gameObj)
	{
		gameController = References.gameController;
		basePosition   = gameObject.transform.position;
	}

	#endregion

	#region Events

	public override void Update()
	{
		base.Update();

		gameObject.transform.position = basePosition + gameController.GetCurrentBobAmount();
	}

	#endregion
}