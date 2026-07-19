using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

	private CameraScaleEffect cameraScaleEffect;
	private PlayerScaleLevel currentScaleLevel;
	private bool isPlaying = true;
	private bool isGameStarted = false;

	private void Awake()
	{
		cameraScaleEffect = GetComponent<CameraScaleEffect>();
		currentScaleLevel = scaleLevels.FirstOrDefault(sl => sl.level == 1);
	}

	public void ScorePoints(int points)
    {
        playerCurrentPoints += points;

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
		playerControl.OnScaleUp(level.targetScale, level.maxLife);

		playerCurrentLevel++;
		currentScaleLevel = level;
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
			Debug.Log("game over");
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
		if (enemiesToDestroy <= 0 && isPlaying)
		{
			isPlaying = false;
			victoryScreen.SetActive(true);
		}
	}

	public void LaunchGame()
	{
		mainMenuScreen.SetActive(false);

		playerControl.gameObject.SetActive(true);
		cameraFollow.ResetPosition();

		isGameStarted = true;
	}
}
