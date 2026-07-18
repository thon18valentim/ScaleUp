using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControl : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float autoDestroyTime = 2f;
	public List<string> tagsWithNoCollision;

	private Rigidbody2D rb;
	private int totalDamage;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		Destroy(gameObject, autoDestroyTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (AvoidColision(collision))
			return;

		if (collision.gameObject.CompareTag("Enemy"))
		{
			var enemyControl = collision.gameObject.GetComponent<EnemyControl>();
			enemyControl.OnDamageReceived(totalDamage);
		}
		else if (collision.gameObject.CompareTag("Turret"))
		{
			var turretControl = collision.gameObject.GetComponent<TurretControl>();
			turretControl.OnDamageReceived(totalDamage);
		}

		Destroy(gameObject);
	}

	public void OnFireShot(int damage)
	{
		totalDamage = damage;
	}

	public void ApplyImpulse(Transform firePoint, float force)
	{
		rb.AddForce(firePoint.up * force, ForceMode2D.Impulse);
	}

	private bool AvoidColision(Collision2D collision)
	{
		foreach (var tag in tagsWithNoCollision)
		{
			if (collision.gameObject.CompareTag(tag))
				return true;
		}

		return false;
	}
}
