using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicsClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.OnGameInitialize += MusicManager_OnGameInitialize;
        GameManager.OnGameStartRound += MusicManager_OnGameStartRound;
    }

    private void OnDisable()
    {
        GameManager.OnGameInitialize -= MusicManager_OnGameInitialize;
        GameManager.OnGameStartRound -= MusicManager_OnGameStartRound;
    }

    private void MusicManager_OnGameInitialize(object sender, EventArgs e)
    {
        if (GameManager.ActualScene.mainMenu == GameManager.Instance.GetActualScene())
        {
            audioSource.clip = musicsClip[0];
            audioSource.Play();
        }
    }

    private void MusicManager_OnGameStartRound(object sender, EventArgs e)
    {
        print("music 2");
        audioSource.clip = musicsClip[1];
        audioSource.Play();
    }
}
