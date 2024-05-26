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
    private ActualScene actualScene;
    public enum ActualScene
    {
        mainMenu,
        Arena,
    }

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

        actualScene = ActualScene.mainMenu;
    }

    public void StartRound()
    {
        OnGameInitialize?.Invoke(this, EventArgs.Empty);
        isGameActive = true;
        isPlayerAlreadySpawn = true;
    }

    public ActualScene GetActualScene()
    {
        return actualScene;
    }
}
