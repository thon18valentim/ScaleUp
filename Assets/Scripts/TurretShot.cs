using System.Collections.Generic;
using UnityEngine;

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

	private bool onDeffenseMode = false;
	private int deffenseModeCount;

	private void Awake()
	{
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
			var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

			var bulletControl = bullet.GetComponent<BulletControl>();
			bulletControl.OnFireShot(totalDamage);
			bulletControl.ApplyImpulse(firePoint, bulletForce);
		}
	}

	public void ActivateDeffenseMode()
	{
		onDeffenseMode = true;
	}
}
