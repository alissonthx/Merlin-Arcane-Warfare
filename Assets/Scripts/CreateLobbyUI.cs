using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createButton;
    private void Awake()
    {
        createButton.onClick.AddListener(() =>
       {
           LobbyManager.Instance.CreateLobby();
           _ = LobbyManager.Instance.StartHostWithRelay();
       });
    }
}
