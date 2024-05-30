using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Space]
    [Header("MainMenu")]
    [SerializeField] private Canvas mainUI;
    [SerializeField] private Canvas createLobbyUI;
    [SerializeField] private Canvas lobbyUI;
    [SerializeField] private Canvas joinLobbyUI;
    [Space]
    [Header("Arena")]
    [SerializeField] private GameObject aimImage;
    [SerializeField] private Canvas loadingUI;

    [Header("Main Buttons")]
    [SerializeField] private Button createMainButton;
    [SerializeField] private Button joinButton;
    [Header("Create Lobby Buttons")]
    [SerializeField] private Button createButton;
    [Header("Join Lobby Buttons")]
    [SerializeField] private Button okButton;

    private void Awake()
    {
        Instance = this;

        InitializeUI();

        if (createMainButton != null)
        {
            createMainButton.onClick.AddListener(() =>
            {
                ShowUI(createLobbyUI);
                HideUI(mainUI);
            });
        }

        if (createButton != null)
        {
            createButton.onClick.AddListener(() =>
            {
                LoadManager.Instance.LoadScene(1);
            });
        }

        if (joinButton != null)
        {
            joinButton.onClick.AddListener(() =>
            {
                ShowUI(lobbyUI);
                HideUI(mainUI);
            });
        }

        if (okButton != null)
        {
            okButton.onClick.AddListener(() =>
            {
                HideUI(joinLobbyUI);
                LoadManager.Instance.LoadScene(1);
            });
        }
    }

    private void OnEnable()
    {
        ActualLobby.OnLobbyClick += UIManager_OnLobbyClick;
    }

    private void OnDisable()
    {
        ActualLobby.OnLobbyClick -= UIManager_OnLobbyClick;
    }

    private void UIManager_OnLobbyClick(object sender, EventArgs e)
    {
        if (joinLobbyUI != null && lobbyUI != null)
        {
            ShowUI(joinLobbyUI);
            HideUI(lobbyUI);
        }
    }

    public void BackUI()
    {
        if (lobbyUI != null) HideUI(lobbyUI);
        if (createLobbyUI != null) HideUI(createLobbyUI);
        if (mainUI != null) ShowUI(mainUI);
    }

    private void InitializeUI()
    {
        // Main menu
        if (mainUI != null) ShowUI(mainUI);
        if (createLobbyUI != null) HideUI(createLobbyUI);
        if (lobbyUI != null) HideUI(lobbyUI);
        if (joinLobbyUI != null) HideUI(joinLobbyUI);

        // Arena
        if (aimImage != null) aimImage.SetActive(false);
        if (PostProcessingEffects.Instance != null) PostProcessingEffects.Instance.BlackWhiteScreen();
    }

    public void StartRoundUI()
    {
        print("start round UI");
        if (aimImage != null) aimImage.SetActive(true);
    }

    private void HideUI(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }
    private void ShowUI(Canvas canvas)
    {
        canvas.gameObject.SetActive(true);
    }
}
