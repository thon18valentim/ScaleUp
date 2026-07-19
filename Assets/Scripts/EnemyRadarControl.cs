using UnityEngine;

public class EnemyRadarControl : MonoBehaviour
{
	[SerializeField] private EnemyTargetSelector targetSelector;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			targetSelector.DetectPlayer(collision.transform);
		}
	}
}
