using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    [SerializeField] private AudioClip[] sfxImpact;
    [SerializeField] private AudioClip[] sfxProjectile;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomImpactSFX(Vector3 position)
    {
        PlayRandomSFXAtPosition(sfxImpact, position);
    }

    public void PlayRandomProjectileSFX(Vector3 position)
    {
        PlayRandomSFXAtPosition(sfxProjectile, position);
    }

    private void PlayRandomSFXAtPosition(AudioClip[] clips, Vector3 position)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips provided.");
            return;
        }

        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clipToPlay = clips[randomIndex];
        AudioSource.PlayClipAtPoint(clipToPlay, position);
    }
}
