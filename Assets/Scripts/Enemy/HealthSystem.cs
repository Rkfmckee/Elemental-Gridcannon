using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
	#region Properties

	public float currentHealth;
	public float maxHealth;
	public float healthBarHeightOffset;

	private new Camera camera;
	private CanvasUI canvas;
	private GameObject healthBarPrefab;
	private GameObject healthBar;
	private HealthBar healthBarController;

	#endregion

	#region Events

	private void Awake()
	{
		healthBarPrefab = Resources.Load<GameObject>("Prefabs/UI/HealthBar");
		currentHealth = maxHealth;
	}

	private void Start()
	{
		canvas = References.UI.canvas;
		camera = References.camera;

		var healthBarParent = canvas.transform.Find("HealthBars").transform;
		healthBar = Instantiate(healthBarPrefab, healthBarParent);
		healthBar.name = $"{name}HealthBar";
		healthBarController = healthBar.GetComponent<HealthBar>();
	}

	private void LateUpdate()
	{
		healthBar.transform.position = camera.WorldToScreenPoint(transform.position + new Vector3(0, healthBarHeightOffset, 0));
		healthBarController.ShowHealthFraction(currentHealth / maxHealth);
	}

	private void OnDestroy()
	{
		Destroy(healthBar);
	}

	#endregion

	#region Methods

	public float GetCurrentHealth()
	{
		return currentHealth;
	}

	public GameObject GetHealthBar()
	{
		return healthBar;
	}

	public void Heal(float amount)
	{
		float newHealth = currentHealth += amount;

		if (newHealth > maxHealth)
		{
			currentHealth = maxHealth;
			return;
		}

		currentHealth = newHealth;
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;

		DestroyIfNoHealth();
	}

	public void TakeDamageOverTime(float totalDamage, float timeInSeconds)
	{
		StartCoroutine(DamageOverTime(totalDamage, timeInSeconds));
	}

	private void DestroyIfNoHealth()
	{
		if (currentHealth <= 0)
		{
			var cardSlot = GetComponent<Enemy>().GetCardSlot();
			var card     = cardSlot.RemoveCard();

			Destroy(card);
			Destroy(gameObject);
		}
	}

	#endregion

	#region Coroutines

	private IEnumerator DamageOverTime(float totalDamage, float timeInSeconds)
	{
		float targetHealth = currentHealth - totalDamage;
		if (targetHealth < 0) { targetHealth = 0; }

		while (currentHealth > targetHealth)
		{
			var decreaseIncrement = (totalDamage / timeInSeconds) * Time.deltaTime;

			if (currentHealth - decreaseIncrement < targetHealth)
			{
				currentHealth = targetHealth;
			}
			else
			{
				currentHealth -= decreaseIncrement;
			}

			yield return null;
		}

		DestroyIfNoHealth();
	}

	#endregion
}
