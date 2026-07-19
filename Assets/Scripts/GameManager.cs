using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CameraScaleEffect))]
public class GameManager : MonoBehaviour
{
	[Header("Match control")]
	[SerializeField] private PlayerControl playerControl;
    [SerializeField] private int playerCurrentPoints = 0;
	[SerializeField] private int playerCurrentLevel = 1;
	[SerializeField] private List<PlayerScaleLevel> scaleLevels;

	[Header("Turrets control")]
	[SerializeField] private List<TurretControl> turrets;

	[Header("Enemies control")]
	[SerializeField] private int enemiesToDestroy;

	[Header("Camera")]
	[SerializeField] private CameraFollow cameraFollow;

	[Header("UI")]
	[SerializeField] private GameObject mainMenuScreen;
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private GameObject victoryScreen;
	[SerializeField] private GameObject creditsModal;
	[SerializeField] private GameObject tutorialModal;
	[SerializeField] private TextMeshProUGUI gameVersionTxt;
	[SerializeField] private List<GameObject> lifeBlocks;
	[SerializeField] private List<GameObject> energyBlocks;
	[SerializeField] private List<GameObject> damageBlocks;
	[SerializeField] private List<GameObject> scaleBlocks;
	[SerializeField] private TextMeshProUGUI enemiesCountTxt;
	[SerializeField] private TextMeshProUGUI currentSpeedTxt;

	private CameraScaleEffect cameraScaleEffect;
	private PlayerScaleLevel currentScaleLevel;
	private bool isPlaying = true;
	private bool isGameStarted = false;

	private void Awake()
	{
		cameraScaleEffect = GetComponent<CameraScaleEffect>();
		currentScaleLevel = scaleLevels.FirstOrDefault(sl => sl.level == 1);
	}

	private void Start()
	{
		gameVersionTxt.text = $"v {Application.version}";
		enemiesCountTxt.text = enemiesToDestroy.ToString();
		currentSpeedTxt.text = "0 u";
	}

	public void ScorePoints(int points)
    {
        playerCurrentPoints += points;
		UpdateEnergyUI(playerCurrentPoints);

		if (playerCurrentPoints >= currentScaleLevel.pointsToLevelUp)
        {
			OnScaleUp();
		}
    }

	private void OnScaleUp()
	{
		var level = scaleLevels.FirstOrDefault(sl => sl.level == playerCurrentLevel + 1);
		if (level == null)
		{
			return;
		}

		cameraScaleEffect.DoEffect(level.targetZoom);
		playerControl.OnScaleUp(level.targetScale, level.maxLife, level.damage);

		playerCurrentLevel++;
		currentScaleLevel = level;

		UpdateScaleUI(playerCurrentLevel);
	}

	public TurretControl TargetAnyTurret()
	{
		if (!turrets.Any())
		{
			return null;
		}

		var rnd = Random.Range(0, turrets.Count);
		return turrets[rnd];
	}

	public bool IsGameStarted()
	{
		return isGameStarted;
	}

	public bool IsGameOver()
	{
		return !isPlaying;
	}

	public bool IsTurretAlive(GUID id)
	{
		var turret = turrets.FirstOrDefault(tr => tr.Id == id);
		return turret != null;
	}

	public void DestroyTurret(GUID id)
	{
		turrets.RemoveAll(rm => rm.Id == id);
		CheckForEndGame();
	}

	private void CheckForEndGame()
	{
		if (turrets.Count == 0 && isPlaying)
		{
			// end game
			Debug.Log("Torretas destruidas");
			isPlaying = false;
			gameOverScreen.SetActive(true);
		}
	}

	public void GameOver()
	{
		isPlaying = false;
		gameOverScreen.SetActive(true);
	}

	public void EnemyDestroyed()
	{
		enemiesToDestroy--;
		enemiesCountTxt.text = enemiesToDestroy.ToString();

		if (enemiesToDestroy <= 0 && isPlaying)
		{
			isPlaying = false;
			victoryScreen.SetActive(true);
		}
	}

	public void LaunchGame()
	{
		CloseCredits();
		CloseTutorial();

		mainMenuScreen.SetActive(false);

		playerControl.gameObject.SetActive(true);
		cameraFollow.ResetPosition();

		isGameStarted = true;
	}

	public void OpenCredits()
	{
		creditsModal.SetActive(true);
	}

	public void CloseCredits()
	{
		creditsModal.SetActive(false);
	}

	public void OpenTutorial()
	{
		tutorialModal.SetActive(true);
	}

	public void CloseTutorial()
	{
		tutorialModal.SetActive(false);
	}

	public void UpdateHealthUI(int health)
	{
		health = Mathf.Clamp(health, 0, lifeBlocks.Count);

		for (int i = 0; i < lifeBlocks.Count; i++)
		{
			var image = lifeBlocks[i].GetComponent<Image>();

			if (image == null)
				continue;

			image.color = i < health ? new Color32(20, 255, 0, 255) : new Color32(142, 142, 142, 255);
		}
	}

	public void UpdateEnergyUI(int points)
	{
		points = Mathf.Clamp(points, 0, energyBlocks.Count);

		for (int i = 0; i < energyBlocks.Count; i++)
		{
			var image = energyBlocks[i].GetComponent<Image>();

			if (image == null)
				continue;

			image.color = i < points ? new Color32(0, 255, 179, 255) : new Color32(142, 142, 142, 255);
		}
	}

	public void UpdateScaleUI(int scale)
	{
		scale = Mathf.Clamp(scale, 0, scaleBlocks.Count);

		for (int i = 0; i < scaleBlocks.Count; i++)
		{
			var image = scaleBlocks[i].GetComponent<Image>();

			if (image == null)
				continue;

			image.color = i < scale ? new Color32(155, 0, 255, 255) : new Color32(142, 142, 142, 255);
		}
	}

	public void UpdateDamageUI(int damage)
	{
		damage = Mathf.Clamp(damage, 0, damageBlocks.Count);

		for (int i = 0; i < damageBlocks.Count; i++)
		{
			var image = damageBlocks[i].GetComponent<Image>();

			if (image == null)
				continue;

			image.color = i < damage ? new Color32(255, 0, 0, 255) : new Color32(142, 142, 142, 255);
		}
	}

	public void UpdateSpeedText(float speed)
	{
		currentSpeedTxt.text = $"{speed.ToString("0.00")} u";
	}
}
