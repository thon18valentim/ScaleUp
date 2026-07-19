using UnityEngine;

[RequireComponent(typeof(EnemyControl))]
public class EnemyTargetSelector : MonoBehaviour
{
	private GameManager gameManager;
	private TurretControl target;
	private Transform playerTarget;
	private bool porsuitPlayer;
	private EnemyControl enemyControl;

	private void Awake()
	{
		enemyControl = GetComponent<EnemyControl>();
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
		if (porsuitPlayer && playerTarget != null)
		{
			return playerTarget;
		}

		if (target == null)
			return null;

		return target.transform;
	}

	public GUID? GetTargetId()
	{
		if (porsuitPlayer)
		{
			return null;
		}

		return target.Id;
	}

	public void DetectPlayer(Transform playerTransform)
	{
		if (!enemyControl.CheckIfCanPorsuitPlayer())
		{
			return;
		}

		playerTarget = playerTransform;
		porsuitPlayer = true;
	}
}
