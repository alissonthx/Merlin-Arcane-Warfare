using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event EventHandler OnGameStartRound;
    public static event EventHandler OnGameInitialize;
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

        OnGameInitialize?.Invoke(this, EventArgs.Empty);
        actualScene = ActualScene.mainMenu;
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
