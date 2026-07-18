using System.Collections;
using UnityEngine;

public class CameraScaleEffect : MonoBehaviour
{
	[Header("Scale Up Camera control")]
	[SerializeField] private float zoomDuration = 0.5f;
	[SerializeField] private AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[SerializeField] private float shakeDuration = 0.2f;
	[SerializeField] private float shakeStrength = 0.15f;

	private Camera cam;
	private Coroutine zoomRoutine;
	private Coroutine shakeRoutine;

	private void Awake()
	{
		cam = Camera.main;
	}

	public void DoEffect(float targetSize)
	{
		SetZoom(targetSize);
		Shake();
	}

	private void SetZoom(float targetSize)
	{
		if (zoomRoutine != null)
			StopCoroutine(zoomRoutine);

		zoomRoutine = StartCoroutine(ZoomRoutine(targetSize));
	}

	private void Shake()
	{
		if (shakeRoutine != null)
			StopCoroutine(shakeRoutine);

		shakeRoutine = StartCoroutine(ShakeRoutine());
	}

	private IEnumerator ZoomRoutine(float targetSize)
	{
		float startSize = cam.orthographicSize;

		float elapsed = 0f;

		while (elapsed < zoomDuration)
		{
			elapsed += Time.deltaTime;

			float t = Mathf.Clamp01(elapsed / zoomDuration);
			float curve = zoomCurve.Evaluate(t);

			cam.orthographicSize = Mathf.LerpUnclamped(
				startSize,
				targetSize,
				curve);

			yield return null;
		}

		cam.orthographicSize = targetSize;
		zoomRoutine = null;
	}

	private IEnumerator ShakeRoutine()
	{
		Vector3 originalPosition = transform.localPosition;

		float elapsed = 0f;

		while (elapsed < shakeDuration)
		{
			elapsed += Time.deltaTime;

			Vector2 offset = Random.insideUnitCircle * shakeStrength;

			transform.localPosition = originalPosition + new Vector3(offset.x, offset.y, 0f);

			yield return null;
		}

		transform.localPosition = originalPosition;
		shakeRoutine = null;
	}
}
