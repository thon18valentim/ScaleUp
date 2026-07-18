using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
	[Header("Fire Points")]
	public List<Transform> firePoints;

	[Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletForce = 20f;
	[SerializeField] private int totalDamage = 20;

	private int shootingSide = 0;

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		AlternateShooting();
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
