using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas mainUI;
    [SerializeField] private Canvas createLobbyUI;
    [SerializeField] private Canvas lobbyUI;
    [SerializeField] private Canvas joinLobbyUI;
    [Header("Main Buttons")]
    [SerializeField] private Button createMainButton;
    [SerializeField] private Button joinButton;
    [Header("Create Lobby Buttons")]
    [SerializeField] private Button createButton;
    [Header("Join Lobby Buttons")]
    [SerializeField] private Button okButton;

    private void Awake()
    {
        InitializeUI();

        createMainButton.onClick.AddListener(() =>
        {
            createLobbyUI.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(false);
        });
        createButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Arena");
        });

        joinButton.onClick.AddListener(() =>
        {
            lobbyUI.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(false);
        });

        okButton.onClick.AddListener(() =>
        {
            joinLobbyUI.gameObject.SetActive(false);
            SceneManager.LoadScene("Arena");
        });
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
        joinLobbyUI.gameObject.SetActive(true);
        lobbyUI.gameObject.SetActive(false);
    }

    public void BackUI()
    {
        lobbyUI.gameObject.SetActive(false);
        createLobbyUI.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);
    }

    private void InitializeUI()
    {
        mainUI.gameObject.SetActive(true);
        createLobbyUI.gameObject.SetActive(false);
        lobbyUI.gameObject.SetActive(false);
        joinLobbyUI.gameObject.SetActive(false);
    }
}
