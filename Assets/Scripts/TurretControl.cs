using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TurretShot))]
public class TurretControl : MonoBehaviour
{
	public GUID Id;

	[Header("Turret Settings")]
	[SerializeField] private int totalLife = 100;

	[Header("Destruction Settings")]
	[SerializeField] private GameObject explosionPrefab;

	[Header("Status")]
	[SerializeField] private int life;

	[Header("Audio Settings")]
	[SerializeField] private AudioSource audioSource;

	private GameManager gameManager;
	private TurretShot turretShot;

	private void Awake()
	{
		Id = GUID.Generate();
		life = totalLife;

		turretShot = GetComponent<TurretShot>();
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

		turretShot.ActivateDeffenseMode();
	}

	private void OnTurretDestroyed()
	{
		var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		explosion.GetComponent<ExplosionEffect>().DoEffect();

		gameManager.DestroyTurret(Id);
		Destroy(gameObject);
	}

	public void PlayShootSound()
	{
		audioSource.Play();
	}
}
