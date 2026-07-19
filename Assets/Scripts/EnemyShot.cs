using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyControl))]
[RequireComponent(typeof(EnemyTargetSelector))]
public class EnemyShot : MonoBehaviour
{
	[Header("Fire Points")]
	[SerializeField] private List<Transform> firePoints;

	[Header("Fire settings")]
	[SerializeField] private ShootingMode shootingMode;
	[SerializeField] private float fireRateCooldown;
	[SerializeField] private float attackCooldown;

	private float currentFireRateCooldown;
	private float currentAttackCooldown;

	[Header("Bullet Settings")]
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletForce = 20f;
	[SerializeField] private int totalDamage = 20;
	[SerializeField] private GameObject bulleetFireEffectPrefab;

	private EnemyControl enemyControl;
	private EnemyTargetSelector enemyTargetSelector;

	private int shootingSide = 0;

	private void Awake()
	{
		enemyControl = GetComponent<EnemyControl>();
		enemyTargetSelector = GetComponent<EnemyTargetSelector>();

		currentFireRateCooldown = fireRateCooldown;
		currentAttackCooldown = attackCooldown;
	}

	private void Update()
	{
		var currentState = enemyControl.GetCurrentState();
		if (currentState == EnemyState.Attacking)
		{
			currentAttackCooldown -= Time.deltaTime;
			if (currentAttackCooldown <= 0)
			{
				currentFireRateCooldown -= Time.deltaTime;
				if (currentFireRateCooldown <= 0)
				{
					Shoot();

					var targetId = enemyTargetSelector.GetTargetId();
					if (targetId != null)
					{
						enemyControl.CheckIfTargetIsAlive((GUID)targetId);
					}

					currentFireRateCooldown = fireRateCooldown;
				}
			}
		}
	}

	public void Shoot()
	{
		if (shootingMode == ShootingMode.Fixed)
		{
			FixedShooting();
		}
		else
		{
			AlternateShooting();
		}

		enemyControl.PlayShootSound();
	}

	private void FixedShooting()
	{
		foreach (var firePoint in firePoints)
		{
			var bulletFireEffect = Instantiate(bulleetFireEffectPrefab, firePoint.position, firePoint.rotation);
			var effect = bulletFireEffect.GetComponent<BulletFireEffect>();
			effect.DoEffect();

			var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

			var bulletControl = bullet.GetComponent<BulletControl>();
			bulletControl.OnFireShot(totalDamage);
			bulletControl.ApplyImpulse(firePoint, bulletForce);
		}
	}

	private void AlternateShooting()
	{
		if (shootingSide >= firePoints.Count)
		{
			shootingSide = 0;
		}

		var firePoint = firePoints[shootingSide];
		var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

		var bulletControl = bullet.GetComponent<BulletControl>();
		bulletControl.OnFireShot(totalDamage);
		bulletControl.ApplyImpulse(firePoint, bulletForce);

		shootingSide++;
	}
}
