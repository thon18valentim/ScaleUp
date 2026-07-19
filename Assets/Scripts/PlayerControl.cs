using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerControl : MonoBehaviour
{
	[Header("Player Settings")]
	[SerializeField] private int totalLife = 10;

	[Header("Scale Up Settings")]
	[SerializeField] private float animationDuration = 0.35f;
	[SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

	[Header("Audio Settings")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioSource scaleUpSource;

	private GameManager gameManager;
	private Coroutine scaleRoutine;
	private PlayerShot playerShot;
	private int currentLife = 0;

	private void Awake()
	{
		currentLife = totalLife;
		playerShot = GetComponent<PlayerShot>();
	}

	private void Start()
	{
		gameManager = FindAnyObjectByType<GameManager>();
		gameManager.UpdateHealthUI(currentLife);
	}

	public void OnScaleUp(float targetScale, int maxLife, int totalDamage)
    {
		if (scaleRoutine != null)
			StopCoroutine(scaleRoutine);

		scaleRoutine = StartCoroutine(ScaleRoutine(targetScale));
		currentLife = maxLife;

		playerShot.UpgradeTotalDamage(totalDamage);

		gameManager.UpdateHealthUI(currentLife);
		gameManager.UpdateDamageUI(totalDamage);
	}

	private IEnumerator ScaleRoutine(float targetScale)
	{
		Vector3 initialScale = transform.localScale;
		Vector3 finalScale = Vector3.one * targetScale;

		float elapsed = 0f;

		while (elapsed < animationDuration)
		{
			scaleUpSource.Play();

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

	public void PlayShootSound()
	{
		audioSource.Play();
	}

	public void OnDamageReceived(int damage)
	{
		currentLife -= damage;
		if (currentLife <= 0)
		{
			Debug.Log("Jogador destruido");
			gameManager.GameOver();
		}

		gameManager.UpdateHealthUI(currentLife);
	}

	public void UpdateSpeedText(float speed)
	{
		gameManager.UpdateSpeedText(speed);
	}
}
