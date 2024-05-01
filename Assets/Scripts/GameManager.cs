using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool isGameActive = false;
    public bool IsGameActive { get => isGameActive; }

    private void Awake()
    {
        Instance = this;
    }

    public void StartRound()
    {
        isGameActive = true;
    }
}