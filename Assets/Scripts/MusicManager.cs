using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicsClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameManager.Instance.GetActualScene() == GameManager.ActualScene.mainMenu)
            audioSource.clip = musicsClip[0];
        else if (GameManager.Instance.GetActualScene() == GameManager.ActualScene.Arena)
            audioSource.clip = musicsClip[1];
    }
}
