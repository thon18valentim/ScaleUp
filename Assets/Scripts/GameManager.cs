using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CameraScaleEffect))]
public class GameManager : MonoBehaviour
{
    [Header("Match control")]
    [SerializeField] private int playerCurrentPoints = 0;
	[SerializeField] private int playerCurrentLevel = 1;
	[SerializeField] private List<PlayerScaleLevel> scaleLevels;

	[Header("Turrets control")]
	[SerializeField] private List<TurretControl> turrets;

	private PlayerControl playerControl;
	private CameraScaleEffect cameraScaleEffect;
	private PlayerScaleLevel currentScaleLevel;

	private void Awake()
	{
		cameraScaleEffect = GetComponent<CameraScaleEffect>();
		currentScaleLevel = scaleLevels.FirstOrDefault(sl => sl.level == 1);
	}

	private void Start()
	{
        playerControl = FindAnyObjectByType<PlayerControl>();
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
		playerControl.OnScaleUp(level.targetScale);

		playerCurrentLevel++;
		currentScaleLevel = level;
	}

	public TurretControl TargetAnyTurret()
	{
		if (!turrets.Any())
		{
			return null;
		}

		return turrets[0];
	}

	public bool IsTurretAlive(GUID id)
	{
		var turret = turrets.FirstOrDefault(tr => tr.Id == id);
		return turret != null;
	}

	public void DestroyTurret(GUID id)
	{
		turrets.RemoveAll(rm => rm.Id == id);
	}
}
