using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas mainUI;
    [SerializeField] private Canvas createLobbyUI;
    [SerializeField] private Canvas lobbyUI;
    [SerializeField] private Canvas joinLobbyUI;
    [Header("Main Menu Buttons")]
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [Header("Join Lobby Buttons")]
    [SerializeField] private Button okButton;

    private void Awake()
    {
        InitializeUI();

        createButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Arena");
            createLobbyUI.gameObject.SetActive(false);
        });

        joinButton.onClick.AddListener(() =>
        {
            lobbyUI.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(false);
        });

        okButton.onClick.AddListener(() =>
        {
            joinLobbyUI.gameObject.SetActive(false);
        });
    }

    public void BackUI()
    {
        lobbyUI.gameObject.SetActive(false);
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
