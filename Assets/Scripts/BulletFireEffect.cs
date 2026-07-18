using UnityEngine;

public class BulletFireEffect : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float autoDestroyTime = 0.1f;

	public void DoEffect()
	{
		Destroy(gameObject, autoDestroyTime);
	}
}
