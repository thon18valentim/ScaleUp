using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[Header("Scale Up Settings")]
	[SerializeField] private float animationDuration = 0.35f;
	[SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

	private Coroutine scaleRoutine;

	public void OnScaleUp(float targetScale)
    {
		if (scaleRoutine != null)
			StopCoroutine(scaleRoutine);

		scaleRoutine = StartCoroutine(ScaleRoutine(targetScale));
	}

	private IEnumerator ScaleRoutine(float targetScale)
	{
		Vector3 initialScale = transform.localScale;
		Vector3 finalScale = Vector3.one * targetScale;

		float elapsed = 0f;

		while (elapsed < animationDuration)
		{
			elapsed += Time.deltaTime;

			float t = Mathf.Clamp01(elapsed / animationDuration);

			float curveValue = scaleCurve.Evaluate(t);

			transform.localScale = Vector3.LerpUnclamped(
				initialScale,
				finalScale,
				curveValue);

			yield return null;
		}

		transform.localScale = finalScale;
		scaleRoutine = null;
	}
}
