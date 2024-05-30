using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event EventHandler OnGameStartRound;
    public static event EventHandler OnGameInitialize;
    public static event EventHandler OnGameWaiting;
    private ActualScene actualScene;
    public enum ActualScene
    {
        mainMenu,
        Waiting,
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
    }
    
    public void MainMenu()
    {
        OnGameInitialize?.Invoke(this, EventArgs.Empty);
        actualScene = ActualScene.mainMenu;
    }

    public void WaitingRound()
    {
        OnGameWaiting?.Invoke(this, EventArgs.Empty);
        actualScene = ActualScene.Waiting;
    }

    public void StartRound()
    {
        OnGameStartRound?.Invoke(this, EventArgs.Empty);
        actualScene = ActualScene.Arena;
    }

    public ActualScene GetActualScene()
    {
        return actualScene;
    }
}
