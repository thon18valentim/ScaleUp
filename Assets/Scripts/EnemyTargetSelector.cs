using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyTargetSelector : MonoBehaviour
{
	private GameManager gameManager;
	private TurretControl target;

	private EnemyMovement enemyMovement;

	private void Awake()
	{
		enemyMovement = GetComponent<EnemyMovement>();
	}

	private void Start()
	{
		gameManager = FindAnyObjectByType<GameManager>();
		OnTargetSelection();
	}

	public void OnTargetSelection()
	{
		target = gameManager.TargetAnyTurret();
	}

	public Transform GetTargetTransform()
	{
		if (target == null)
			return null;

		return target.transform;
	}

	public GUID GetTargetId()
	{
		return target.Id;
	}
}
