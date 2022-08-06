using UnityEngine;

public class EnemyStateIdle : EnemyState
{
	#region Properties

	private Vector3 bottomPosition;
	private Vector3 topPosition;
	private BobDirection currentBobDirection;
	private float bobTime;
	private float bobTimer;

	#endregion

	#region Constructors

	public EnemyStateIdle(GameObject gameObj) : base(gameObj)
	{
		bottomPosition      = gameObject.transform.position;
		topPosition         = bottomPosition + new Vector3(0, 0.2f, 0);
		currentBobDirection = BobDirection.Up;
		bobTime             = 1f;
		bobTimer            = 0f;

		Debug.Log(bottomPosition);
	}

	#endregion

	#region Events

	public override void Update()
	{
		base.Update();

		Debug.Log(bottomPosition);

		switch (currentBobDirection)
		{
			case BobDirection.Up:
				Bob(bottomPosition, topPosition, BobDirection.Down);
				break;
			case BobDirection.Down:
				Bob(topPosition, bottomPosition, BobDirection.Up);
				break;
		}
	}

	#endregion

	#region Methods

	private void Bob(Vector3 startPosition, Vector3 endPosition, BobDirection nextDirection)
	{
		gameObject.transform.position  = Vector3.Lerp(startPosition, endPosition, bobTimer / bobTime);
		bobTimer                      += Time.deltaTime;

		Debug.Log(endPosition);

		if (Vector3.Distance(gameObject.transform.position, endPosition) == 0)  {
			currentBobDirection = nextDirection;
			bobTimer = 0;
		}
	}

	#endregion

	#region Enums

	public enum BobDirection
	{
		None,
		Up,
		Down
	}

	#endregion
}