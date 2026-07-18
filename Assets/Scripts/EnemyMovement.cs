using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(EnemyTargetSelector))]
[RequireComponent(typeof(EnemyControl))]
public class EnemyMovement : MonoBehaviour
{
	[Header("Attributes")]
	[SerializeField] private float moveSpeed = 2.5f;
	[SerializeField] private float attackRange = 5f;

	[Header("Rotation")]
	[SerializeField] private float rotationSpeed = 25f;

	private EnemyTargetSelector targetSelector;
	private EnemyControl enemyControl;

	private void Awake()
	{
		targetSelector = GetComponent<EnemyTargetSelector>();
		enemyControl = GetComponent<EnemyControl>();
	}

	private void Update()
	{
		Move();
	}

	private bool IsInAttackRange()
	{
		var target = targetSelector.GetTargetTransform();
		if (target == null)
			return false;

		return Vector2.Distance(transform.position, target.position) <= attackRange;
	}

	private void Move()
	{
		var target = targetSelector.GetTargetTransform();
		if (target == null)
			return;

		if (IsInAttackRange())
		{
			enemyControl.SetState(EnemyState.Attacking);
			return;
		}

		enemyControl.SetState(EnemyState.Moving);

		Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
		transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

		Rotate(direction);
	}

	private void Rotate(Vector2 direction)
	{
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			Quaternion.Euler(0, 0, angle),
			360f * Time.deltaTime  * rotationSpeed);
	}
}
