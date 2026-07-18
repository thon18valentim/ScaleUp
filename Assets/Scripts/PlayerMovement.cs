using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Attributes")]
	[SerializeField] private float speed;

	[Header("Rotation")]
	[SerializeField] private float rotationSpeed = 720f;

	private Rigidbody2D rb;
	private Camera mainCamera;

	private Vector2 mouseWorldPosition;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		mainCamera = Camera.main;
	}

	private void Update()
	{
		mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	private void FixedUpdate()
	{
		Move();
		Rotate();
	}

	private void Move()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		Vector2 direction = new Vector2(x, y).normalized;

		Vector2 position = transform.position;
		position += speed * Time.deltaTime * direction;

		transform.position = position;
	}

	private void Rotate()
	{
		Vector2 direction = mouseWorldPosition - rb.position;

		if (direction.sqrMagnitude < 0.001f)
			return;

		float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

		float angle = Mathf.MoveTowardsAngle(
			rb.rotation,
			targetAngle,
			rotationSpeed * Time.fixedDeltaTime);

		rb.MoveRotation(angle);
	}
}
