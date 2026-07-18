using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretControl))]
public class TurretShot : MonoBehaviour
{
	[Header("Fire Points")]
	[SerializeField] private List<Transform> firePoints;

	[Header("Fire settings")]
	[SerializeField] private float fireRateCooldown;
	[SerializeField] private int deffenseModeRate;

	private float currentFireRateCooldown;

	[Header("Bullet Settings")]
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletForce = 20f;
	[SerializeField] private int totalDamage = 20;
	[SerializeField] private GameObject bulleetFireEffectPrefab;

	private bool onDeffenseMode = false;
	private int deffenseModeCount;

	private TurretControl turretControl;

	private void Awake()
	{
		turretControl = GetComponent<TurretControl>();

		currentFireRateCooldown = fireRateCooldown;
		deffenseModeCount = deffenseModeRate;
	}

	private void Update()
	{
		if (onDeffenseMode)
		{
			currentFireRateCooldown -= Time.deltaTime;
			if (currentFireRateCooldown <= 0)
			{
				Shoot();
				currentFireRateCooldown = fireRateCooldown;

				deffenseModeCount++;
				if (deffenseModeCount > deffenseModeRate)
				{
					deffenseModeCount = 0;
					onDeffenseMode = false;
				}
			}
		}
	}

	public void Shoot()
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

			turretControl.PlayShootSound();
		}
	}

	public void ActivateDeffenseMode()
	{
		onDeffenseMode = true;
	}
}
