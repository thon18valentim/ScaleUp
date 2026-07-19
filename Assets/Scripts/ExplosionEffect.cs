using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExplosionEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float destroyTime = 3f;

	public void DoEffect()
    {
        audioSource.Play();
        Destroy(gameObject, destroyTime);
	}
}
