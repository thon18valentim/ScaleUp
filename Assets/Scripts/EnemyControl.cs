using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(EnemyTargetSelector))]
public class EnemyControl : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private int totalLife = 100;
	[SerializeField] private int points = 1;
	[SerializeField] private EnemyBehavior enemyBehavior;

	[Header("Status")]
	[SerializeField] private int life;

	[Header("States")]
	[SerializeField] private EnemyState currentState;

	private GameManager gameManager;
	private EnemyTargetSelector targetSelector;

	private void Awake()
	{
		targetSelector = GetComponent<EnemyTargetSelector>();

		life = totalLife;
		currentState = EnemyState.Moving;
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
			OnEnemyDestroyed();
		}
	}

	private void OnEnemyDestroyed()
	{
		gameManager.ScorePoints(points);
		Destroy(gameObject);
	}

	public EnemyState GetCurrentState()
	{
		return currentState;
	}

	public void SetState(EnemyState state)
	{
		currentState = state;
	}

	public void CheckIfTargetIsAlive(GUID targetId)
	{
		var isTargetAlive = gameManager.IsTurretAlive(targetId);
		if (!isTargetAlive)
		{
			currentState = EnemyState.Moving;
			targetSelector.OnTargetSelection();
		}
	}
}
