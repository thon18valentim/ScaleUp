using UnityEngine;

public class TurretControl : MonoBehaviour
{
	public GUID Id;

	[Header("Turret Settings")]
	[SerializeField] private int totalLife = 100;

	[Header("Status")]
	[SerializeField] private int life;

	private GameManager gameManager;

	private void Awake()
	{
		Id = GUID.Generate();
		life = totalLife;
	}

	private void Start()
	{
		gameManager = FindAnyObjectByType<GameManager>();
	}

	public void OnDamageReceived(int damage)
	{
		life -= damage;
		if (life <= 0)
		{
			OnTurretDestroyed();
		}
	}

	private void OnTurretDestroyed()
	{
		gameManager.DestroyTurret(Id);
		Destroy(gameObject);
	}
}
