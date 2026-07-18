using TMPro;
using UnityEngine;

public class CoordinatesControl : MonoBehaviour
{
	[SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI coordinatesTxt;

	private void Update()
	{
		Vector2 position = player.position;
		coordinatesTxt.text = $"X: {position.x:F1} Y: {position.y:F1}";
	}
}
