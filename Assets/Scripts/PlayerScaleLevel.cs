using System;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public class PlayerScaleLevel
	{
		[SerializeField] public int level;
		[SerializeField] public int pointsToLevelUp;
		[SerializeField] public float targetScale;
		[SerializeField] public float targetZoom;
	}
}
