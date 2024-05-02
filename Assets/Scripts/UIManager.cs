using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas mainUI;
    [SerializeField] private Canvas createLobbyUI;
    [SerializeField] private Canvas lobbyUI;
    [SerializeField] private Button create;
    [SerializeField] private Button join;

    private void Awake()
    {
        InitializeUI();

        create.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Arena");
            createLobbyUI.gameObject.SetActive(false);
        });
        join.onClick.AddListener(() =>
        {
            lobbyUI.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(false);
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
        lobbyUI.gameObject.SetActive(false);
        createLobbyUI.gameObject.SetActive(false);
    }
}
