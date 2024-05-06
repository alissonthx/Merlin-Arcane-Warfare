using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event EventHandler OnGameInitialize;

    private bool isGameActive = false;
    private bool isPlayerAlreadySpawn = false;

    public bool IsGameActive => isGameActive;
    public bool IsPlayerAlreadySpawn => isPlayerAlreadySpawn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartRound()
    {
        OnGameInitialize?.Invoke(this, EventArgs.Empty);
        isGameActive = true;
        isPlayerAlreadySpawn = true;
    }
}
