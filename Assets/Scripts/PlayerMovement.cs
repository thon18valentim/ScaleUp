using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Attributes")]
	[SerializeField] private float moveSpeed = 5f;

	[Header("Rotation")]
	[SerializeField] private float rotationSpeed = 720f;

	[Header("Movement Settings")]
	[SerializeField] private Transform planet;
	[SerializeField] private float mapRadius = 40f;

	[Header("Engine Trail Settings")]
	[SerializeField] private Transform engineTrail;
	[SerializeField] private float minLength = 0.3f;
	[SerializeField] private float maxLength = 1.5f;
	[SerializeField] private float smooth = 8f;

	[Header("Speed Trail Settings")]
	[SerializeField] private List<Transform> speedTrails;
	[SerializeField] private float speedMinLength = 0.15f;
	[SerializeField] private float speedMaxLength = 1.0f;
	[SerializeField] private float speedSmooth = 8f;

	private Camera mainCamera;
	private Rigidbody2D rb;
	private Vector2 mouseWorldPosition;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		mainCamera = Camera.main;
	}

	private void Update()
	{
		HandleInWorldMovement();
		HandleMousePosition();

		ClampPosition();
	}

	private void FixedUpdate()
	{
		HandleRotation();
	}

	private void HandleInWorldMovement()
	{
		var x = Input.GetAxisRaw("Horizontal");
		var y = Input.GetAxisRaw("Vertical");

		Vector2 direction = new Vector2(x, y).normalized;

		Vector2 position = transform.position;
		position += moveSpeed * Time.deltaTime * direction;

		transform.position = position;

		DoEngineTrail(direction);
		DoSpeedTrail(direction);
	}

	private void HandleMousePosition()
	{
		mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	private void HandleRotation()
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

	private void ClampPosition()
	{
		Vector2 direction = (Vector2)transform.position - (Vector2)planet.position;

		if (direction.magnitude > mapRadius)
		{
			direction = direction.normalized * mapRadius;

			transform.position = (Vector2)planet.position + direction;
		}
	}

	private void DoEngineTrail(Vector2 direction)
	{
		float speedPercent = direction.magnitude;

		Vector3 scale = engineTrail.localScale;

		scale.y = Mathf.Lerp(
			scale.y,
			Mathf.Lerp(minLength, maxLength, speedPercent),
			smooth * Time.deltaTime);

		engineTrail.localScale = scale;
	}

	private void DoSpeedTrail(Vector2 direction)
	{
		float speedPercent = direction.magnitude;

		foreach (var speedTrail in speedTrails)
		{
			Vector3 scale = speedTrail.localScale;

			scale.y = Mathf.Lerp(
				scale.y,
				Mathf.Lerp(speedMinLength, speedMaxLength, speedPercent),
				speedSmooth * Time.deltaTime);

			speedTrail.localScale = scale;
		}
	}
}
